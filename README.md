# Inventory and Shop

This Unity project implements a flexible Inventory and Shop system suitable for RPGs, survival games, and other item-based games. It uses the Model-View-Controller (MVC) pattern to cleanly separate game logic, data, and UI.

## Features

- 📦 Inventory system with weight and size limits
- 🛒 Shop system for buying and selling items
- 🧩 Modular `ScriptableObject`-based items
- 🎨 Item rarities, types, and icons
- 💡 Tooltips with item details
- 🧠 MVC architecture for maintainability and scalability

## Architecture

- **Model**: Handles inventory data like current weight, size, and item list
- **View**: Manages Unity UI and visual updates
- **Controller**: Mediates between user actions and model updates

## Item Structure

Each item is a `ScriptableObject` with the following fields:

- Name, Description
- Type (Weapon, Consumable, etc.)
- Rarity (Common, Rare, Epic, etc.)
- Weight, Quantity
- Buy/Sell Prices
- Icon