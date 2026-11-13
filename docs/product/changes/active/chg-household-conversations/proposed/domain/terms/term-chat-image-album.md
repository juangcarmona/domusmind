---
id: TERM-CHAT-IMAGE-ALBUM
type: domain-term
title: Chat image album
status: draft
defined-in: BC-CHATS
uses-terms:
  - TERM-CHAT-ATTACHMENT
  - TERM-CHAT-MESSAGE
---

## Definition

A presentation grouping for consecutive image attachments sent in one conversation action or within the accepted grouping window, while retaining each image as an individually authorized attachment.

## Boundaries

Album presentation does not merge attachment permissions, replace individual download behavior or make an image visible to anyone who could not access it separately.

## Distinguish From

- An album is a presentation grouping, not a single combined file.
- Non-image attachments and images that do not meet the grouping policy remain separate message attachments.

## Usage

The product applies the accepted image type, miniature, grouping-window and fallback behavior while retaining each attachment's independent lifecycle.
