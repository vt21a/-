using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//абстрактен клас
public abstract class SmartDevice
{
    public string Name { get; set; }
    public double EnergyConsumption { get; set; }

    //абстрактни методи за включване и изключване на устройството
    public abstract void TurnOn();
    public abstract void TurnOff();
}

//цлас смартЛигхт
public class SmartLight : SmartDevice
{
    public int Brightness { get; set; }  // яркостта
    public string Color { get; set; }  // цвят

    public SmartLight(string name, double energyConsumption, int brightness, string color)
    {
        Name = name;
        EnergyConsumption = energyConsumption;
        Brightness = brightness;
        Color = color;
    }

    public override void TurnOn()
    {
        Console.WriteLine($"Осветлението {Name} е включено.");
    }

    public override void TurnOff()
    {
        Console.WriteLine($"Осветлението {Name} е изключено.");
    }
}

//клас умен термостат
public class SmartThermostat : SmartDevice
{
    public double CurrentTemperature { get; set; }
    public double DesiredTemperature { get; set; }

    public SmartThermostat(string name, double energyConsumption, double currentTemperature, double desiredTemperature)
    {
        Name = name;
        EnergyConsumption = energyConsumption;
        CurrentTemperature = currentTemperature;
        DesiredTemperature = desiredTemperature;
    }

    public override void TurnOn()
    {
        Console.WriteLine($"Термостат {Name} е включен, настроен на {DesiredTemperature}°C.");
    }

    public override void TurnOff()
    {
        Console.WriteLine($"Термостат {Name} е изключен.");
    }
}

//клас за управление на умния дом
public class SmartHome : IEnumerable<SmartDevice>
{
    private List<SmartDevice> devices;

    public SmartHome()
    {
        devices = new List<SmartDevice>();
    }

    //метод за добавяне на устройства
    public void AddDevice(SmartDevice device)
    {
        devices.Add(device);
    }

    //итератор чрез IEnumerable
    public IEnumerator<SmartDevice> GetEnumerator()
    {
        return devices.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

//компаратор за сортиране по енергията
public class EnergyConsumptionComparer : IComparer<SmartDevice>
{
    public int Compare(SmartDevice x, SmartDevice y)
    {
        return x.EnergyConsumption.CompareTo(y.EnergyConsumption);
    }
}

//компаратор за сортиране по име
public class NameComparer : IComparer<SmartDevice>
{
    public int Compare(SmartDevice x, SmartDevice y)
    {
        return x.Name.CompareTo(y.Name);
    }
}

//Main клас
public class Program
{
    //метод за извличане на методи и свойства на клас
    public static void DisplayDeviceInfo(SmartDevice device)
    {
        Type type = device.GetType();
        Console.WriteLine($"\nИнформация за устройството {device.Name} ({type.Name}):");

        Console.WriteLine("Публични свойства:");
        foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            object value = prop.GetValue(device);
            Console.WriteLine($"- {prop.Name}: {value}");
        }

        Console.WriteLine("Публични методи:");
        // Show only methods defined in this class (not inherited or property-related)
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                          .Where(m => !m.IsSpecialName); // filters out get_/set_ etc.

        foreach (var method in methods)
        {
            Console.WriteLine($"- {method.Name}()");
        }
    }


    public static void Main(string[] args)
    {
        //Създаване на обекти от различните класове устройства
        SmartDevice light1 = new SmartLight("Smart Light 1", 15, 80, "White");
        SmartDevice light2 = new SmartLight("Smart Light 2", 10, 50, "Blue");
        SmartDevice thermostat1 = new SmartThermostat("Thermo 1", 30, 22, 25);
        SmartDevice thermostat2 = new SmartThermostat("Thermo 2", 40, 18, 22);

        //създаване на инстанция на умния дом
        //добавяне устройства
        SmartHome home = new SmartHome();
        home.AddDevice(light1);
        home.AddDevice(light2);
        home.AddDevice(thermostat1);
        home.AddDevice(thermostat2);

        //използване на итератора за избор на устройствата
        Console.WriteLine("Всички устройства в умния дом:");
        foreach (var device in home)
        {
            Console.WriteLine($"Устройство: {device.Name}, Консумация на енергия: {device.EnergyConsumption}W");
        }

        //сортиране по енергопотребление
        Console.WriteLine("\nСортиране по енергопотребление:");
        var energyComparer = new EnergyConsumptionComparer();
        var sortedByEnergy = home.ToList();
        sortedByEnergy.Sort(energyComparer);
        foreach (var device in sortedByEnergy)
        {
            Console.WriteLine($"Устройство: {device.Name}, Консумация: {device.EnergyConsumption}W");
        }

        //сортиране по име
        Console.WriteLine("\nСортиране по име:");
        var nameComparer = new NameComparer();
        var sortedByName = home.ToList();
        sortedByName.Sort(nameComparer);
        foreach (var device in sortedByName)
        {
            Console.WriteLine($"Устройство: {device.Name}, Консумация: {device.EnergyConsumption}W");
        }

        //използване на reflection за извличане на информацията на устройствата
        Console.WriteLine("\nИнформация за устройствата чрез reflection:");
        DisplayDeviceInfo(light1);
        DisplayDeviceInfo(thermostat1);

    }



}
