# Meal Planning Feature Specification

## 1. Product Concept

### What Meal Planning Means in DomusMind

Meal planning in DomusMind is a family-centered, weekly coordination system that enables households to plan, organize, and execute their meals with minimal friction. It focuses on:

- **Weekly reuse over creation** - Templates and previous weeks are the foundation
- **Family-level coordination** - Meals are planned for the household, not individuals
- **Integration with existing surfaces** - Seamlessly connects with Agenda and Lists
- **Low variability** - Stable weekday meals with flexible weekend options

### What It Is NOT

- A recipe database or social platform
- A nutritional analysis tool
- A complex meal planning algorithm
- An individual diet management system

### Core User Value

Families can plan their meals weekly with minimal effort, reuse proven patterns, and generate shopping lists automatically - all within their existing DomusMind workflow.

## 2. Domain Model

### Core Entities

#### MealPlan
- **Purpose**: A weekly aggregation of meals for a household
- **Attributes**: 
  - Id (UUID)
  - WeekStart (Date)
  - FamilyId (UUID)
  - CreatedAt (DateTime)
  - UpdatedAt (DateTime)
- **Relationships**: 
  - Contains multiple MealSlots
  - References a WeeklyTemplate (optional)
- **Lifecycle**: Created weekly, potentially reused, modified as needed
- **Invariants**: 
  - Must have exactly 7 MealSlots (one per day)
  - Must belong to exactly one family
  - WeekStart must represent a Monday

#### MealSlot
- **Purpose**: A single meal time slot within a day
- **Attributes**:
  - Id (UUID)
  - DayOfWeek (Enum: Monday-Sunday)
  - MealType (Enum: Breakfast, Lunch, Dinner, Snack)
  - MealPlanId (UUID)
  - RecipeId (UUID, optional)
  - Notes (String, optional)
- **Relationships**:
  - Belongs to exactly one MealPlan
  - May reference exactly one Recipe
- **Lifecycle**: Created with MealPlan, can be assigned or modified
- **Invariants**:
  - Must belong to exactly one day of the week
  - Must have exactly one meal type
  - Must belong to exactly one MealPlan

#### Recipe
- **Purpose**: A collection of ingredients and preparation steps for a meal
- **Attributes**:
  - Id (UUID)
  - Name (String)
  - Description (String, optional)
  - PrepTimeMinutes (Integer, optional)
  - CookTimeMinutes (Integer, optional)
  - Servings (Integer, optional)
  - FamilyId (UUID)
  - CreatedAt (DateTime)
  - UpdatedAt (DateTime)
- **Relationships**:
  - Contains multiple Ingredients
  - Can be referenced by multiple MealSlots
- **Lifecycle**: Created manually or imported (initially manual)
- **Invariants**:
  - Must belong to exactly one family
  - Name must be unique within family

#### Ingredient
- **Purpose**: A component of a recipe
- **Attributes**:
  - Id (UUID)
  - Name (String)
  - Quantity (Decimal)
  - Unit (String, optional)
  - RecipeId (UUID)
- **Relationships**:
  - Belongs to exactly one Recipe
- **Lifecycle**: Created with Recipe, modified as needed
- **Invariants**:
  - Must belong to exactly one Recipe
  - Name must be unique within Recipe (for deduplication)

#### ShoppingList
- **Purpose**: A derived list of ingredients needed for meals
- **Attributes**:
  - Id (UUID)
  - Name (String)
  - FamilyId (UUID)
  - CreatedAt (DateTime)
  - UpdatedAt (DateTime)
  - GeneratedFromMealPlanId (UUID, optional)
- **Relationships**:
  - Contains multiple ShoppingListItem
  - Derived from one or more MealPlans
- **Lifecycle**: Automatically generated, manually editable
- **Invariants**:
  - Is derived, never manually authoritative
  - Must belong to exactly one family
  - Must reference exactly one MealPlan (when generated)

#### ShoppingListItem
- **Purpose**: A single item in a shopping list
- **Attributes**:
  - Id (UUID)
  - IngredientId (UUID)
  - Quantity (Decimal)
  - Unit (String, optional)
  - Notes (String, optional)
  - ShoppingListId (UUID)
- **Relationships**:
  - Belongs to exactly one ShoppingList
  - References exactly one Ingredient
- **Lifecycle**: Created during ShoppingList derivation, can be manually edited
- **Invariants**:
  - Must belong to exactly one ShoppingList
  - Must reference exactly one Ingredient

#### WeeklyTemplate
- **Purpose**: A reusable pattern for weekly meal planning
- **Attributes**:
  - Id (UUID)
  - Name (String)
  - FamilyId (UUID)
  - CreatedAt (DateTime)
  - UpdatedAt (DateTime)
- **Relationships**:
  - Contains multiple MealSlotTemplates
  - Can be applied to create new MealPlans
- **Lifecycle**: Created once, reused many times
- **Invariants**:
  - Must belong to exactly one family
  - Must have exactly 7 MealSlotTemplates (one per day)

#### MealSlotTemplate
- **Purpose**: A template for a specific meal slot
- **Attributes**:
  - Id (UUID)
  - DayOfWeek (Enum: Monday-Sunday)
  - MealType (Enum: Breakfast, Lunch, Dinner, Snack)
  - RecipeId (UUID, optional)
  - Notes (String, optional)
  - WeeklyTemplateId (UUID)
- **Relationships**:
  - Belongs to exactly one WeeklyTemplate
  - May reference exactly one Recipe
- **Lifecycle**: Created with WeeklyTemplate, modified as needed
- **Invariants**:
  - Must belong to exactly one day of the week
  - Must have exactly one meal type
  - Must belong to exactly one WeeklyTemplate

#### DietaryConstraint
- **Purpose**: Information about dietary restrictions or preferences
- **Attributes**:
  - Id (UUID)
  - Name (String)
  - Description (String, optional)
  - FamilyId (UUID)
- **Relationships**:
  - Can be associated with Members or Recipes
- **Lifecycle**: Created once, reused
- **Invariants**:
  - Must belong to exactly one family
  - Name must be unique within family

#### FamilyPreference
- **Purpose**: Household-wide meal preferences and settings
- **Attributes**:
  - Id (UUID)
  - FamilyId (UUID)
  - PreferredMealTypes (List of MealType)
  - WeekendFlexibility (Boolean)
  - DefaultDietaryConstraints (List of DietaryConstraintId)
- **Relationships**:
  - Belongs to exactly one Family
- **Lifecycle**: Created once, modified as needed
- **Invariants**:
  - Must belong to exactly one family

### Aggregates

1. **MealPlan** - Root aggregate containing MealSlots
2. **Recipe** - Root aggregate containing Ingredients
3. **ShoppingList** - Root aggregate containing ShoppingListItems
4. **WeeklyTemplate** - Root aggregate containing MealSlotTemplates
5. **FamilyPreference** - Root aggregate with Family settings

### Ownership Boundaries

- All meal planning entities belong to a Family
- MealPlans and Templates are household-level constructs
- Recipes and Ingredients are family-level but can be shared across multiple meal plans
- ShoppingLists are derived from MealPlans and belong to the household

## 3. Temporal Modeling

### Mapping to Agenda

Meals are **derived projections** of MealPlans, not separate entities in the Calendar context. They appear in Agenda through the Lists system.

### Day Structure

Each day contains:
- Breakfast (optional)
- Lunch (optional)
- Dinner (required)
- Snack (optional)

### Handling Repetition, Templates, and Overrides

- **Templates** are reusable patterns for weekly meal planning
- **Repetition** is handled through WeeklyTemplates that get instantiated into MealPlans
- **Overrides** are handled by allowing users to modify specific MealSlots within a MealPlan

### Source of Truth vs Projections

- **Source of Truth**: MealPlans, Recipes, WeeklyTemplates
- **Projections**: ShoppingLists (automatically derived), Agenda entries (through Lists integration)

### Sync Rules with Agenda

- MealSlots project into Lists as list items with temporal fields
- Lists with temporal fields project into Agenda as plans
- ShoppingLists are generated from MealPlans and can be viewed in the Lists surface

## 4. Integration with Existing Systems

### Agenda Integration

- **Visual Representation**: Meal slots appear as list items in the Lists surface, which projects into Agenda
- **Grouping**: Meals are grouped by day in the Lists surface, with potential for filtering by meal type
- **Interaction Model**: Clicking a meal slot opens the Inspector for editing
- **Editing Flow**: Direct editing in the Lists surface or via Inspector

### Lists Integration

- **Shopping List Generation**: Automatically generated from MealPlans when needed
- **Deduplication Rules**: Same ingredients across recipes are combined with summed quantities
- **Manual vs Auto Items**: Shopping list items can be manually edited after auto-generation
- **List Item Projection**: MealSlot items project into Lists with due dates and recurrence

### Areas Integration

- **Food Area**: Meals fall under the "Cooking" or "Food Preparation" area
- **Ownership**: Responsibility for meal planning is shared among household members
- **Boundaries**: Meal planning is part of the broader "Household Operations" area

## 5. UX Model

### Weekly Planning Flow

1. **Week Creation**: Users create a new MealPlan for a week starting on Monday
2. **Previous Week Reuse**: Users can copy or adapt from previous weeks
3. **Template Application**: Users can apply WeeklyTemplates to quickly set up a week
4. **Default Behaviors**: 
   - Weekdays default to template-based meals
   - Weekends default to flexible arrangements
   - All meals have basic structure but allow customization

### Meal Assignment

1. **Drag/Drop**: Not supported in initial release - manual assignment only
2. **Quick Assign**: Direct assignment from template or recipe to a slot
3. **Default Behaviors**: 
   - Pre-filled with template defaults when available
   - Flexible weekend slots with no defaults

### Variability Control

1. **Stable Weekdays**: Fixed meals that are reused across weeks
2. **Flexible Weekends**: Open slots for spontaneous meal choices
3. **Override Mechanism**: Users can override any meal slot in a MealPlan

### Inspector Behavior

1. **Editing a Meal**: Modify recipe, notes, or timing
2. **Editing a Recipe**: Update ingredients, prep/cook time, serving size
3. **Editing a Slot**: Change meal type, recipe, or notes
4. **Template Management**: View and edit templates for reuse

### Shopping Flow

1. **Automatic Generation**: Shopping list generated when user clicks "Generate Shopping List"
2. **Manual Editing**: Users can adjust quantities or add/remove items
3. **Integration**: Shopping list appears in Lists surface as a regular list
4. **Visibility**: Shopping list shows which MealPlan it was generated from

## 6. Data Flow & Derivations

### What is Stored

- MealPlans (weekly meal aggregations)
- Recipes (ingredient compositions)
- WeeklyTemplates (reusable patterns)
- ShoppingLists (derived from MealPlans)
- DietaryConstraints (household preferences)
- FamilyPreferences (household settings)

### What is Computed

- ShoppingLists (from MealPlans and Recipe ingredients)
- Agenda projections (from Lists with temporal fields)

### Derivation Triggers

1. **MealPlan Creation** → Generate ShoppingList (optional)
2. **MealSlot Assignment** → Update ShoppingList (if already generated)
3. **Recipe Modification** → Update ShoppingList (if already generated)
4. **Template Application** → Create new MealPlan

### Idempotency Rules

- Applying a template to a MealPlan is idempotent (applying twice has same effect as applying once)
- Generating a ShoppingList from the same MealPlan is idempotent (generating twice produces identical results)
- Modifying a MealSlot in a MealPlan preserves derived ShoppingList but allows manual adjustments

## 7. Architecture Impact

### Backend Changes

#### New Aggregates

1. **MealPlan** aggregate (with MealSlots)
2. **Recipe** aggregate (with Ingredients)
3. **ShoppingList** aggregate (with ShoppingListItems)
4. **WeeklyTemplate** aggregate (with MealSlotTemplates)
5. **FamilyPreference** entity

#### New Services

1. **MealPlanningService** - Coordinates meal plan operations
2. **ShoppingListGenerationService** - Generates shopping lists from meal plans
3. **TemplateService** - Manages weekly templates

#### Required Handlers

1. **CreateMealPlanCommandHandler**
2. **UpdateMealSlotCommandHandler**
3. **ApplyWeeklyTemplateCommandHandler**
4. **GenerateShoppingListCommandHandler**
5. **CreateRecipeCommandHandler**
6. **UpdateRecipeCommandHandler**
7. **CreateWeeklyTemplateCommandHandler**

### Read Models

#### Extensions to Agenda Read Model

- Extended with meal-related list items projected from MealPlans
- Enhanced filtering capabilities for meal types
- Improved grouping by day of week

#### Performance Considerations

- Caching of frequently accessed templates
- Efficient ingredient aggregation for shopping lists
- Pre-computed shopping list derivations where possible

### Frontend Changes

#### New Components (Minimal)

1. **MealPlanEditor** - Weekly meal planning interface
2. **RecipeEditor** - Recipe creation and modification
3. **ShoppingListViewer** - Shopping list display and editing

#### Reuse of Existing Components

1. **Inspector** - For detailed editing of meals, recipes, and templates
2. **Agenda Views** - For displaying meal-related list items
3. **Lists UI** - For shopping list presentation
4. **DateNavigator** - For selecting weeks to plan

## 8. Anti-Goals

1. **Full Recipe Social Platform** - No social features, reviews, or sharing beyond family
2. **Nutritional Micromanagement** - No calorie tracking or nutritional analysis
3. **Per-User Diet Optimization Engine** - Focus on family-level preferences, not individual diets
4. **Complex Calorie Tracking UI** - Simplified ingredient and quantity management only
5. **External Integration** - No recipe import or external API integrations in initial release

## 9. Opportunity Space

### What Competitors Overcomplicate

1. **Recipe databases** - DomusMind avoids deep recipe collections
2. **Nutritional analysis** - Focuses on practical meal planning, not health metrics
3. **Personalized diets** - Family-level planning, not individual profiles
4. **Social features** - No sharing beyond family units

### What They Fail to Solve

1. **Family coordination** - Most apps focus on individuals or recipes
2. **Weekly reuse patterns** - Many apps require daily planning from scratch
3. **Shopping list generation** - Often disconnected from meal planning
4. **Household workflow integration** - Not embedded in existing household systems

### Where DomusMind Wins

1. **Family-first approach** - Integrates seamlessly with existing family structures
2. **Weekly reuse** - Templates and previous weeks reduce planning effort
3. **Integration with existing surfaces** - No new surfaces, just enhanced Lists
4. **Low friction** - Minimal ceremony, easy to start and maintain

## 10. Product Principles

1. **Weekly reuse over creation** - Templates and history are the foundation
2. **Default over configuration** - Smart defaults reduce decision fatigue
3. **Derived over manual** - Shopping lists auto-generate, not manually maintained
4. **Household over individual** - Family-level planning, not personal preferences
5. **Integration over feature isolation** - Leverage existing Lists and Agenda surfaces
6. **Simplicity over complexity** - Minimal UI, clear workflow

## 11. Vertical Slice Definition

### Scope

#### Must Include

1. **Weekly MealPlan** - Basic weekly meal aggregation with 7 slots
2. **MealSlots** - Dinner-only initially (simplified for MVP)
3. **Manual Recipe Entry** - Simple recipe creation with ingredients
4. **Shopping List Generation** - Automatic derivation from MealPlan
5. **Basic Templates** - Simple weekly pattern templates

#### Must Exclude

1. **Advanced Nutrition** - No calorie tracking or nutritional analysis
2. **AI Suggestions** - No machine learning or smart recommendations
3. **External Integrations** - No recipe imports or API connections
4. **Complex Dietary Constraints** - Basic dietary preferences only
5. **Multi-person Planning** - Single household planning only

### UI Location

#### Agenda
- Meal-related list items project into Agenda
- No direct meal planning in Agenda surface

#### Lists
- MealPlan items appear as list items
- ShoppingList items appear as list items
- Both surfaces use existing Lists UI

#### Inspector
- Detailed editing of meals, recipes, and templates
- Uses existing Inspector pattern with new content

### Implementation Priority

1. **MealPlan Creation** - Basic weekly plan creation
2. **Recipe Management** - Simple recipe creation and editing
3. **Shopping List Generation** - Derive from MealPlans
4. **Template System** - Create and apply weekly templates
5. **UI Integration** - Connect to Lists and Agenda surfaces