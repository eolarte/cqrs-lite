---
name: proj-bootstrap
description: Guide the user through the initial setup of the project by explaining the project's structure, key documentation, and how to get started. Use when a user is new to the project and needs orientation.
user-invocable: true
---

Ask the user about what the project is about and what they want to achieve with it. 

Then, guide them through the following steps:

1. Project Overview
2. Concept / Product Description
3. Project goals and vision
4. Tech Stack

Create the documentation area, if does not exist:

```
docs/ 
  contexts/ 
    context-map.md - bounded contexts and relationships
    shared-glossary.md - canonical terminology used across the project
  specs/ - specifications and requirements
  guides/ - user guides and tutorials
```

Then produce a document in `docs/project-brief.md` with the following structure, filling in as much detail as possible based on the conversation:


``` markdown 
# project-brief.md — [Project Name]

## 1. Project Overview

[3-4 sentences describing what the project is, who it's for, and the core goal.]

## 2. Concept / Product Description

[Detailed description of the product — user flows, key features, narrative if applicable.]

## 3. Project goals and vision
- **Short-term goals:** [Immediate milestones for the next sprint or two]

## 4. Tech Stack

- **Codebase:** [language, key libraries, runtime, framework]
- **Hosting:** [platform, CDN, storage]
- **Testing:** [test framework, E2E tool]
- **CI/CD:** [pipeline tool]
```

## Related documentation
- `context-map.md` — for understanding the different contexts in the project and how they relate
- `shared-glossary.md` — for understanding the canonical terminology used across the project
- `AGENTS.md` — for understanding the different agents in the project and their roles

** Note: update `context-map.md` and `shared-glossary.md` as needed in case of changes to the project's structure or terminology. **

## Related skills
- `research` — for gathering information about the project and its domain
- `interview` — for asking the user about their vision and goals for the project
- `spec-prd-writing` - for producing the final feature PRD document in a structured format.
