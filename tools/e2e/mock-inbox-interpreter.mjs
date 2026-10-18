import { createServer } from "node:http";

const responseFields = {
  description: null,
  date: null,
  time: null,
  endDate: null,
  endTime: null,
  dueDate: null,
  unresolvedDetails: null,
  targetPlanTitle: null,
  reminderMinutesBefore: null,
};

function proposal(type, title, fields = {}) {
  return { type, title, ...responseFields, ...fields };
}

function interpret(note) {
  let match = note.match(/Task:\s*(.+?)\. Due date:\s*(\d{4}-\d{2}-\d{2})\./i);
  if (match) return [proposal("Task", match[1], { dueDate: match[2] })];

  match = note.match(/Plan:\s*(.+?)\. Start date:\s*(\d{4}-\d{2}-\d{2})\. End date:\s*(\d{4}-\d{2}-\d{2})\./i);
  if (match) return [proposal("Plan", match[1], { date: match[2], endDate: match[3] })];

  match = note.match(/Reminder:.*?(\d+) minutes?.*?on (\d{4}-\d{2}-\d{2})/i);
  if (match) return [proposal("Reminder", "Add a reminder", { reminderMinutesBefore: Number(match[1]) })];

  match = note.match(/Timeline note:.*?on (\d{4}-\d{2}-\d{2})/i);
  if (match) return [proposal("TimelineNote", "First Visit to Camp Muir", { date: match[1] })];

  return [];
}

const server = createServer(async (request, response) => {
  if (request.method === "GET" && request.url === "/health") {
    response.writeHead(200).end("ok");
    return;
  }

  if (request.method !== "POST" || request.url !== "/v1/chat/completions") {
    response.writeHead(404).end();
    return;
  }

  let body = "";
  for await (const chunk of request) body += chunk;
  const payload = JSON.parse(body);
  const note = payload.messages?.filter((message) => message.role === "user").at(-1)?.content ?? "";
  if (note.includes("Member request:\n")) {
    const answer = note.includes("Pay summer camp fee")
      ? "The household timeline includes Pay summer camp fee."
      : "I could not find that task in the authorized household data.";
    const content = JSON.stringify({ answer, proposal: null });
    response.writeHead(200, { "content-type": "application/json" }).end(JSON.stringify({
      choices: [{ message: { content } }],
    }));
    return;
  }
  const noteText = note.split("Note:\n").at(-1) ?? "";
  const content = JSON.stringify({ proposals: interpret(noteText) });

  response.writeHead(200, { "content-type": "application/json" }).end(JSON.stringify({
    choices: [{ message: { content } }],
  }));
});

server.listen(Number(process.env.PORT ?? 39002), "127.0.0.1");
