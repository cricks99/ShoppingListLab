Dictionary<string, decimal> itemList = new Dictionary<string, decimal>()
{
    { "Milk", (decimal)4.80 },
    { "Pickles", (decimal)5.22 },
    { "Cheese", (decimal)2.49 },
    { "Hamburger (1 pound)", (decimal)12.58 },
    { "Corn", (decimal)1.49 },
    { "Green Beans", (decimal).98 },
    { "Cabbage", (decimal)3.49 },
    { "Cream Soda", (decimal).79 },
    { "Bottle of Water", (decimal)1.49 },
    { "Box of Rocks (Large)", (decimal)152.29 }
};

List<string> shoppingCart = new List<string>();
var menuList = itemList.OrderBy(x => x.Key).Select(x => x.Key).ToList();
menuList.Add("Cancel Order");

bool orderAgain = true;

Console.WriteLine("Welcome to My Market!");

while (orderAgain)
{   
    Console.WriteLine();
    DisplayMenu();

    orderAgain = AddItem() && AskOrderMoreItems();
}

if (shoppingCart.Count() > 0)
    ShowOrder();
else
    Console.WriteLine("\nSorry to see you go without placing an order.  Hope to see you again soon.");

Console.WriteLine("\nHave a great day!");
Console.ReadKey();
Environment.Exit(0);

void DisplayMenu()
{
    Console.Clear();
    Console.WriteLine("Here are the items that can be ordered:\n");
    Console.WriteLine($"   {"#", 3} {"Item", -20} {"Price", 8}");
    Console.WriteLine($"   {"-".PadRight(3, '-')} {"-".PadRight(20, '-')} {"-".PadRight(8, '-')}");

    for (int i = 0; i < menuList.Count; i++)
    {
        Console.Write($"   {i + 1,3} {menuList[i], -20} ");
    
        if (i < menuList.Count - 1)
            Console.WriteLine($"   {itemList[menuList[i]], 8:c}");
        else
            Console.WriteLine();  //this is for the last cancel order line -- no price
    }

    if (shoppingCart.Count > 0)
    {
        var orderTotal = shoppingCart.Select(x => new KeyValuePair<string, decimal>(x, itemList[x])).Select(x => x.Value).Sum();
        Console.WriteLine($"\nItems in your cart: {shoppingCart.Count}");
        Console.WriteLine($"Cart total: {orderTotal:c}");
    }
}

bool AddItem()
{
    int itemNumber = -1;
    bool isValid = false;

    while (!isValid)
    {
        Console.Write("\nWhat item would you like to order? ");
        try
        {
            itemNumber = int.Parse(Console.ReadLine());

            if (!(isValid = itemNumber > 0 && itemNumber <= menuList.Count()))
                Console.WriteLine($"Only item choices from 1 to {menuList.Count()} can be chosen.");
        }
        catch(FormatException)
        {
            Console.WriteLine("That's not a valid menu choice.  A menu number is expected.");
        }
    }

    if (itemNumber == menuList.Count())
        return false;

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Adding {menuList[itemNumber - 1]} to cart at {itemList[menuList[itemNumber - 1]]:c}.");
    Console.ResetColor();
    shoppingCart.Add(menuList[itemNumber - 1]);
    
    return true;
}

bool AskOrderMoreItems()
{
    string userInput;

    while (true)
    {
        Console.Write("\nWould you like to order anything else (Y/n)? ");
        userInput = Console.ReadLine(); 

        if (userInput.ToLower() != "y" && userInput.ToLower() != "n" && userInput != "")
        {
            Console.WriteLine("I don't understand.  Try again.");
            continue;
        }
        
        return userInput.ToLower() == "y" || userInput == "";
    }
}

void ShowOrder()
{
    Console.Clear();
    Console.WriteLine("\nThank you for your order!\n");

    Console.WriteLine($"{"-".PadRight(30, '-')}");
    Console.WriteLine("Here are your order details (ordered by price):");

    var finalOrder = shoppingCart.Select(x => new KeyValuePair<string, decimal>(x, itemList[x])).OrderBy(x => x.Value);

    foreach (KeyValuePair<string, decimal> item in finalOrder)
        Console.WriteLine($"     {item.Key, -20} {item.Value, 8:c}");
    Console.WriteLine($"\nOrder Total: {finalOrder.Select(x => x.Value).Sum():c}");
    Console.WriteLine($"{"-".PadRight(30, '-')}");

    if (finalOrder.Select(x => x.Key).FirstOrDefault() != finalOrder.Select(x => x.Key).LastOrDefault())
    {
        Console.WriteLine($"\nYour least expensive item ordered is {finalOrder.Select(x => x.Key).FirstOrDefault()}.");
        Console.WriteLine($"Your most expensive item ordered is {finalOrder.Select(x => x.Key).LastOrDefault()}.");
        Console.WriteLine($"Your average price per item in your order is {finalOrder.Select(x => x.Value).Average():c}.");
    }
}