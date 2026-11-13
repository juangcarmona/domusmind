---
title: Surface Spec — Chats
status: Canonical
audience: Product / Design / Engineering
scope: Household conversations and explicit read-only assistant questions
---

# Surface Spec — Chats

## Purpose

Chats gives household members one place to coordinate in household conversations and private groups, with an explicitly invoked, read-only `@dm` assistant.

<!-- pdac:cite id="BC-CHATS" digest="sha256:8bee3e6eae0eef994be3523b72a983cd4c98e8b3aa1d365da90fd7de7d24283f" -->
<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:ca38e0324cedc282b3ad87f3f482eb85bf0772b6aeb509468f9b18a4559a7418" -->

## Layout

Desktop uses the standard shell with a compact page header, a conversation list at the left, and the selected conversation in the main canvas. The conversation list shows the title and whether the thread is household-wide or a private group. The selected conversation shows messages in time order and a composer at the bottom.

On mobile, the same conversation list and message view stack vertically. The list stays compact and scrollable; message content and the composer use the available width without horizontal scrolling.

## Conversation creation

The page header has one primary action, **New conversation**. The creation panel asks for a title and conversation type. Household conversations include all household members. Private groups let the creator select the other participants; the creator is added automatically. The panel can be dismissed without saving.

## Messages and assistant use

Messages show the author's name and local time. Assistant replies are visually distinguished and remain in the conversation where they were requested. The composer identifies the `@dm` invocation and has a 4000-character limit.

Chats shows an external-provider notice when the selected assistant sends questions and household context outside the deployment. Ordinary messages are not sent to the assistant.

<!-- pdac:cite id="BR-CHAT-ASSISTANT-READ-ONLY" digest="sha256:1908dad987dce6d293efd2ed5de7928b2d57eb788d868102461ade53c74dc7cf" -->
<!-- pdac:cite id="QR-CHAT-PRIVATE-CONTENT" digest="sha256:b58936e309e9fea5898eddefd2c44a8c6eebdb25d268add2f59a3d2c3667baca" -->

## Privacy and access

Only participants can find, read or post to a private group. A rejected request does not reveal the title or message content. The page does not show member details for a thread the current member cannot access.

<!-- pdac:cite id="BR-CHAT-MEMBER-ACCESS" digest="sha256:4a477d90cec98082be4894fe5c77a009172de5cb9a127d4ab379c795c5c7bce6" -->
