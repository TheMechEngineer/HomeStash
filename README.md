# HomeStash

## Overview
HomeStash is a desktop application designed to manage and organize items within a building.  
It allows users to create buildings, rooms, containers, and items, and track where everything is stored.

The system is built with a structured front-end and back-end architecture, using adapters to separate UI concerns from core data logic.

---

## Features

- Create and manage Users
- Create and manage Buildings
- Create and manage Rooms within Buildings
- Create and manage Containers within Rooms or other Containers
- Add, modify, and move Items
- Visual Top-Down Building View with Grid Layout
- Dynamic UI Controls with consistent layout behavior
- Data persistence using JSON (via Data Continuity system)
- Report generation for building contents

---

## Architecture

### Front End
- Built using Windows Forms
- Uses UserControls for all UI components (except Dashboard)
- Consistent layout and sizing logic across all controls
- Adapter pattern used to standardize UI data handling

### Back End
- Handles all core data models and business logic
- Includes:
  - RootManager
  - User
  - Building
  - Room
  - Container
  - Item

### Adapters
Adapters are used to bridge UI and data:
- `AdapterSelection`
- `AdapterSelectionItem`
- `ComboBoxLineItem`

These ensure:
- Separation of UI and domain logic
- Consistent display formatting
- Type-safe data handling

---

## Key Design Decisions

### 1. Separation of Concerns
UI logic is completely separated from business logic:
- FrontEnd handles display and interaction
- BackEnd handles data and rules

### 2. Adapter Pattern
Used to:
- Avoid relying on `ToString()` for UI display
- Provide consistent data structures to UI controls
- Support multiple object types (User, Building, Room, Container)

### 3. Dynamic Layout System
All UserControls:
- Calculate their own layout at runtime
- Use consistent spacing variables (`Gap`, `SmallGap`)
- Align labels and inputs uniformly

### 4. Event-Driven Updates
UI automatically updates based on backend changes:
- UserListChanged
- BuildingListChanged
- Custom event wiring per control

### 5. Form Modes
Forms operate in two modes:
- Add
- Modify

Controlled via:
```csharp
FormType
