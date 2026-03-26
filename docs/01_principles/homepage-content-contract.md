# DomusMind — Homepage Content Contract

## 1. Purpose

Define the **content structure, constraints, and messaging invariants** of the homepage.

This is not marketing copy.

This is a **contract** that guarantees:

* clarity
* consistency
* alignment with the domain model
* resistance to generic SaaS drift

---

## 2. Core Invariant

Every section must reinforce:

> **The house stops depending on one person’s memory.**

If a section does not reinforce this, it is invalid.

---

## 3. Problem Definition (Non-negotiable)

The homepage must express:

* households operate as systems
* but system state lives in one person’s head
* that person:

  * remembers
  * coordinates
  * anticipates
  * reminds

This creates:

* invisible work
* imbalance
* fragility

Anchor phrases (allowed):

* “estar pendiente de todo”
* “llevar la casa en la cabeza”
* “acordarse de todo lo de todos”

---

## 4. Product Definition

DomusMind is:

* a **shared operational system for the household**

It does:

* make state visible
* make responsibility explicit
* unify time, tasks, and coordination

It is:

* not a task app
* not a calendar

But:

* do not rely on negation alone
* always replace with **positive definition**

---

## 5. Tone Constraints

Must:

* use concrete situations
* use human language
* use short sentences
* show, not describe

Must not:

* use abstract terms without grounding:

  * “infraestructura”
  * “carga cognitiva”
* sound like documentation
* sound like a productivity tool

---

## 6. Structural Sections (Fixed Order)

---

### 6.1 Hero

**Intent**

Immediate recognition of the problem.

**Structure**

* line 1 → reality
* line 2 → system shift
* line 3 → outcome

**Anchors**

* one person holds everything
* dependency
* shared system

---

### 6.2 Reality (Problem)

**Intent**

User must feel:

> “this is exactly my house”

**Content**

* concrete situations
* examples of “being pending of everything”

---

### 6.3 Breakdown

**Intent**

Explain why this happens.

**Key idea**

Tools fail because they:

* track tasks
* but not responsibility
* not coordination
* not system state

---

### 6.4 The Shift

**Intent**

Introduce the new model.

**Content**

* shared state
* explicit ownership
* visible coordination

---

### 6.5 What You See (Critical Section)

**Intent**

Make the product visible.

**Must include**

A “Today” view.

Format example:

```
Hoy

Mateo → Natación 18:00
Marta → Reunión colegio 09:00

Comprar leche
Revisar mochila

Luna → Veterinario
```

Derived from timeline model.

---

### 6.6 How It Works

**Intent**

Explain system behavior without technical language.

**Flow**

1. something happens / is added
2. becomes part of the household
3. appears in timeline
4. connects to people and responsibilities

---

### 6.7 What Exists Today (V1 Scope)

Must reflect actual system:

* family
* members
* responsibilities
* events (plans)
* tasks
* routines
* timeline

No invented features.

---

### 6.8 Audience Split (Secondary)

Primary: families
Secondary: developers

Developers message:

* open system
* can extend
* can contribute

But:

* never dominate the page
* never break narrative

---

## 7. Example System Data (MANDATORY)

Use realistic, localized names.

---

### 7.1 Given Names by Region

| Region  | Parents (80s)    | Children | Pet    |
| ------- | ---------------- | -------- | ------ |
| Spain   | Javier, Marta    | Mateo    | Luna   |
| Germany | Thomas, Anna     | Leon     | Bella  |
| UK/US   | Michael, Sarah   | Liam     | Max    |
| France  | Nicolas, Julie   | Gabriel  | Nala   |
| Italy   | Marco, Francesca | Sofia    | Luna   |
| Japan   | Takashi, Yuki    | Haruto   | Momo   |
| China   | Wei, Li          | Yichen   | BaoBao |

---

### 7.2 Family Naming Patterns

| Region  | Example                 |
| ------- | ----------------------- |
| Spain   | Familia García Martínez |
| Germany | Familie Müller          |
| UK/US   | The Smith Family        |
| France  | Famille Martin          |
| Italy   | Famiglia Rossi          |
| Japan   | 佐藤家 (Satō-ke)           |
| China   | 王家 (Wáng jiā)           |

---

### 7.3 Rules

* always use culturally consistent names per locale
* avoid generic names
* avoid placeholders
* prefer recognizable family structures

---

## 8. Example Content Rules

Good:

* “Marta tiene que estar pendiente de todo.”
* “Si Marta deja de pensar en la casa, algo falla.”

Bad:

* “gestión del hogar”
* “optimización de tareas”
* “infraestructura compartida”

---

## 9. Visual Contract (Non-optional)

The homepage must not be markdown-like.

Must include:

* “Today” board visual
* grouped items
* separation between:

  * plans
  * tasks
  * responsibilities

Avoid:

* long text blocks
* documentation style sections

---

## 10. Final Constraint

This page is a **projection of the domain model into human language**.

If it becomes:

* abstract
* generic
* tool-like

It is wrong.
