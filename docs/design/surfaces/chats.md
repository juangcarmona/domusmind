---
title: Surface Spec — Chats
status: Canonical
audience: Product / Design / Engineering
scope: Household conversations and explicit read-only assistant questions
---

# Surface Spec — Chats

## Purpose

Chats gives household members one place to coordinate in household conversations and private groups, with an explicitly invoked, read-only `@dm` assistant.

<!-- pdac:cite id="BC-CHATS" digest="sha256:02ba26543d23ee6f89430f2766bcd7c86f3e4cc4e3fcad3307a75d94a475310e" -->
<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:471f9fa8993fa17314d158ee599bfbe772b0b10251665aabcedb4f99ec16d486" -->

## Layout

Desktop uses the standard shell with a compact page header, a conversation list at the left, and the selected conversation in the main canvas. The conversation list shows the title and whether the thread is household-wide or a private group. The selected conversation shows messages in time order and a composer at the bottom.

On mobile, the same conversation list and message view stack vertically. The list stays compact and scrollable; message content and the composer use the available width without horizontal scrolling.

## Conversation creation

The page header has one primary action, **New conversation**. The creation panel asks for a title and conversation type. Household conversations include all household members. Private groups let the creator select the other participants; the creator is added automatically. The panel can be dismissed without saving.

## Messages and assistant use

Messages show the author's name and local time. Assistant replies are visually distinguished and remain in the conversation where they were requested. The composer identifies the `@dm` invocation and has a 4000-character limit.

Chats shows an external-provider notice when the selected assistant sends questions and household context outside the deployment. Ordinary messages are not sent to the assistant.

<!-- pdac:cite id="BR-CHAT-ASSISTANT-READ-ONLY" digest="sha256:0d3703a9b0b3d2e5db5baf8b8c8658a40739273f42c973e17943aec214b4d6c8" -->
<!-- pdac:cite id="QR-CHAT-PRIVATE-CONTENT" digest="sha256:2a21faf96ceea5d0842fdc28b3308c404d49f9862d15a6f646f9132f45adfea3" -->

## Privacy and access

Only participants can find, read or post to a private group. A rejected request does not reveal the title or message content. The page does not show member details for a thread the current member cannot access.

<!-- pdac:cite id="BR-CHAT-MEMBER-ACCESS" digest="sha256:fa5a7ef2fc77a8b8a3c55da99567c801445606f9ecc263bafff5a0261c0bb15a" -->
