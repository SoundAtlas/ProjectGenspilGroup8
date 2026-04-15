# Project Genspil Group 8

## Project Overview
This project is a C# console application developed as part of a system development and programming course.  
The system is designed for **Genspil**, a company that sells second-hand board games and needs a better way to manage their inventory.

Currently, Genspil manages their inventory using a long and unstructured Word document, which makes it difficult to maintain an overview of games, stock levels, and customer requests.  
This system replaces that approach with a structured and searchable inventory system.

---

## Purpose
The purpose of the system is to:
- Provide a clear overview of available games
- Support efficient searching using multiple criteria
- Manage stock information (condition, price, quantity)
- Handle customer requests for games
- Enable export of inventory data

---

## Features
The system implements the following core use cases:

- **Add Game**
  - Create new games or update existing ones
  - Add stock information (condition, price, quantity)

- **Search Games**
  - Search using one or more criteria:
    - Name (partial match supported)
    - Genre
    - Number of players
    - Condition
    - Price range

- **Register Request**
  - Register customer requests for games
  - Automatically create the game if it does not exist

- **View Inventory**
  - Display all games in the system
  - Sorting options:
    - Name
    - Genre
    - No sorting

- **Update Game**
  - Modify existing game and stock information

- **Export Inventory**
  - Export inventory list to file
  - Supports sorting before export

---

## Technologies Used
- C# (.NET)
- Console application
- Object-Oriented Programming (OOP)
- File-based persistence

---

## How to Run the Project

1. Clone the repository:
   git clone https://github.com/SoundAtlas/ProjectGenspilGroup8.git

2. Open the solution file:
   ProjectGenspilGroup8.slnx

3. Run the project in Visual Studio

4. Use the menu in the console to navigate the system

---

## Project Structure

ProjectGenspilGroup8/

├── ProjectGenspilGroup8/   # Main application code  
├── docs/                  # Reports and design models  
├── README.md  
├── .gitignore  
└── ProjectGenspilGroup8.slnx  

---

## Documentation
The `/docs` folder contains:
- System development report
- Programming report
- Design models (Domain model, Class diagram, Sequence diagrams, BPMN, Use Cases & Systeme Sequence Diagrams)

These documents demonstrate the traceability from business requirements to implementation.

---

## Traceability
The system is developed based on defined use cases, and there is a clear connection between:
- Business requirements  
- Domain model  
- Use cases  
- Sequence diagrams  
- Class diagram  
- Code implementation  

This ensures consistency between analysis, design, and implementation.

---

## Authors
Group 8 – Project Genspil
- Andreas Hemmer
- Anika Fuglsang
- Emmerence Steffesen
- Kristoffer Hov
- Østen Andreas Helm Boa
---

## Status
The system fulfills the requirements described in the project case and supports inventory management, search, requests, and data export.
