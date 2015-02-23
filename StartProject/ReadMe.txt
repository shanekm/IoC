// IoC - specific PATTER used to invert interfaces, flow and dependencies
// DI - IMPLEMENTATION of IoC to invert dependencies
// IoC Container - FRAMEWORK to do dependency injection

// Examples of Dependency Inversion -> Portable chargers
// Inteface is used as contract between High level class and Low level class

// Problem of COPY -> read/writer dependency on different input/outout/writers and readers
// Button - Lamp => Button implements IButtonClient (turn on/off) => Lamp (Button doesn't have to depend on Lamp and vice versa)

// Normal object/class creation => Created inside object that uses it
// ex. MyClass myObject = new MyClass()
// ex. IMyInterface myObject = new MyInterfaceImplementation() (even though it's an interface there is no IoC, no benefit)
// Inverted object/class createion => To break dependancy create objects OUTSIDE the object that uses it
// ex. Constructor injection

// TYPES OF CREATION INVERSION
// 1. Factory Pattern => Button button = ButtonFactory.CreateButton();
// 2. Service Locator => Button button = ServiceLocator.Create(IButton.class) => create a button object for me, any object that implements IButton
// 3. Dependency Injection => Button button = GetTheRightButton();
// and more
// ANY TIME you move creation of an object outside of class you are doing dependency inversion

// What is Dependency Injection => move creation and binding of dependency OUTSIDE of the class that depends on it
// MyClass (depends on IDependency) => IDependency => 'Dependency concrete' implements IDependency => How is Dependency created? passed to MyClass??
// Where do dependencies come from? => Injector

Button button;
switch (UserSettings.UserSkinType)
{
    case UserSkinType.Normal:
        button = new Button();
        break;
    case UserSkinType.Fancy:
        button = new FancyButton();
        break;
    ...
}

// Factory pattern => control inverted from class using button to factory
Button button = ButtonFactory.CreateButton();

What is IoC Container? => a framework for doing dependency injection
- configure dependencies
- automatically resolves configures dependencies (when I have this dependency resolve to this concrete type)