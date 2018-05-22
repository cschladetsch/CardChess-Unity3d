# App.Registry

Generic dependancy injection system with instance tracking, persistence and lifetime management.

Used to manage *Models*, *Agents* and *Views*.

Objects added to a **Registry\<T\>** are given a unique identifier, and have an `OnDestroyed` event.

Objects can be created via `Reg.New<T>(args...)`, or at construction time using the `[Inject]` property attribute.
 
Circular references are allowed via injection, so you can have two classes that each *inject* an instance of each other.

The purpose of using different **Registries** to handle the three different architectural layers of *Model*, *Agent* and *View* is that one common system (with different parameterised base types) can be used for each.
