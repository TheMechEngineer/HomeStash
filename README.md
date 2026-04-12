# HomeStash

## Overview
HomeStash is a Windows desktop application used to organize and track items within a hierarchical structure consisting of buildings, rooms, containers, and items.

The system is designed to model real-world storage layouts in a structured digital format.

---

## Core Concept

The application represents storage using the following hierarchy:

- **Building**
  - Top-level container (e.g., House, Storage Unit)
- **Room**
  - Subdivision within a building (e.g., Garage, Bedroom)
- **Container**
  - Storage objects inside rooms or other containers (e.g., Boxes, Shelves)
- **Item**
  - Individual objects stored within containers or rooms

Each level maintains a parent-child relationship to preserve location tracking.

---

## Features

### Data Organization
- Create and manage users
- Create buildings associated with users
- Create rooms within buildings
- Create containers within rooms or other containers
- Add items with defined properties and locations

### Item Management
- Add, modify, and move items between locations
- Track item metadata (name, description, value, quantity)
- Assign items to any valid storage location

### UI Behavior
- Form-based input for all entity types
- Add and modify modes supported across forms
- Dynamic layout sizing based on control content
- Consistent alignment across user controls

### Data Handling
- Uses a centralized backend model structure
- Storage relationships maintained through interfaces and parent references
- UI updates driven by event-based notifications from the backend

---

## Application Flow

1. Application starts at `Program.cs`
2. Main dashboard is loaded
3. A user context is selected or created
4. Buildings are created under the user
5. Rooms and containers are added to build structure
6. Items are assigned to storage locations
7. Modifications propagate through event updates

---

## UI Structure

The application is built using Windows Forms with modular UserControls:

- `UserInfo` – User creation/modification
- `BuildingInfo` – Building management
- `RoomInfo` – Room management
- `ItemInfo` – Item creation/modification/movement
- `Dashboard` – Primary navigation interface

Each control handles:
- Input validation
- Layout sizing
- Add/Modify state behavior via `FormType`

---

## Data Model Relationships

- A **User** can own multiple **Buildings**
- A **Building** contains multiple **Rooms** and top-level **Containers**
- A **Room** can contain **Containers** and **Items**
- A **Container** can contain nested **Containers** and **Items**
- An **Item** references its immediate parent location via `IStorageHolder`

---

## Key Implementation Notes

- Uses event-driven updates for UI synchronization
- Adapter classes are used to bridge UI components and backend models
- Custom combo box line items are used to associate display text with data objects
- Recursive traversal is used for nested container resolution

---

## Project Structure

- **FrontEnd**
  - Forms (Dashboard)
  - UserControls (UI components)
  - Adapters (UI-data mapping utilities)
  - Utilities (custom controls, helpers)

- **BackEnd**
  - ModelClasses (core entities)
  - ModelInterfaces (shared contracts)
  - Enumerations
  - Data management logic

---

## Technologies

- C#
- Windows Forms (.NET)
- Object-Oriented Design
- Event-driven architecture
- Interface-based hierarchy modeling

---

## Notes

- The system assumes a strict hierarchical storage model
- Items are always assigned to a single parent location
- Containers support recursive nesting

---
