using Lab2Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2Db
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("== Book Paradise ==");
                Console.WriteLine("1. Show inventory");
                Console.WriteLine("2. Add book to store");
                Console.WriteLine("3. Delete book from store");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ShowInventory();
                        break;
                    case 2:
                        Console.Clear();
                        AddToBookStore();
                        break;
                    case 3:
                        Console.Clear();
                        DeleteBookFromStore();
                        break;

                    case 4:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again");
                        break;
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();

            }
            static void ShowInventory()
            {
                using var db = new Lab2DbContext();

                Console.WriteLine("== Inventory ==");

                var inventoryList = db.LagerSaldon.Include(ls => ls.Butik).Include(ls => ls.IsbnNavigation).ToList();

                foreach (var item in inventoryList)
                {
                    Console.WriteLine($"{item.Butik.ButikensNamn} - {item.IsbnNavigation.Titel}: {item.Antal} st");
                }
            }

            static void AddToBookStore()
            {
                using var db = new Lab2DbContext();
                var shop = db.Butikerna.ToList();

                Console.WriteLine("== All Stores ==\n");

                for (int i = 0; i < shop.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {shop[i].ButikensNamn}");
                }

                Console.Write("Select a store: ");

                int storeChoice = int.Parse(Console.ReadLine()) - 1;
                var selectedStore = shop[storeChoice];

                var inventory = db.LagerSaldon.Where(ls => ls.ButikId == selectedStore.Id).Include(ls => ls.IsbnNavigation).ToList();

                Console.Clear();

                Console.WriteLine($"== Books in {selectedStore.ButikensNamn} ==");

                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {inventory[i].IsbnNavigation.Titel} (Amount: {inventory[i].Antal})");
                }

                Console.Write("Select a book: ");
                int bookChoice = int.Parse(Console.ReadLine()) - 1;

                var selectedBook = inventory[bookChoice];

                Console.WriteLine($"\nYou selected: {selectedBook.IsbnNavigation.Titel} ");
                Console.Clear();
                Console.Write("Enter amount you want to add: ");
                int amountToAdd = int.Parse(Console.ReadLine());

                selectedBook.Antal += amountToAdd;

                db.SaveChanges();

                Console.WriteLine($"\nAdded {amountToAdd} of {selectedBook.IsbnNavigation.Titel} to {selectedStore.ButikensNamn}. New amount: {selectedBook.Antal}.");
            }


            static void DeleteBookFromStore()
            {
                using var db = new Lab2DbContext();
                var shop = db.Butikerna.ToList();

                Console.WriteLine("== All Stores ==\n");

                for (int i = 0; i < shop.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {shop[i].ButikensNamn}");
                }

                Console.Write("\nSelect a store: ");

                int storeChoice = int.Parse(Console.ReadLine()) - 1;
                var selectedStore = shop[storeChoice];

                var inventory = db.LagerSaldon
                    .Where(ls => ls.ButikId == selectedStore.Id)
                    .Include(ls => ls.IsbnNavigation)
                    .ToList();

                Console.Clear();
                Console.WriteLine($"== Books in {selectedStore.ButikensNamn} ==\n");

                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {inventory[i].IsbnNavigation.Titel} (Amount: {inventory[i].Antal})");
                }

                Console.Write("\nSelect a book: ");
                int bookChoice = int.Parse(Console.ReadLine()) - 1;

                var selectedBook = inventory[bookChoice];

                Console.Clear();
                Console.WriteLine($"You selected: {selectedBook.IsbnNavigation.Titel}");
                Console.WriteLine($"Current amount: {selectedBook.Antal}");

                Console.Write("\nEnter amount you want to delete: ");
                int amountToDelete = int.Parse(Console.ReadLine());

                if (selectedBook.Antal >= amountToDelete)
                {
                    selectedBook.Antal -= amountToDelete;
                }
                else
                {
                    Console.WriteLine("Cant remove more than the store inventory, try again");
                    return;
                }


                db.SaveChanges();

                Console.WriteLine($"\nRemoved {amountToDelete} of '{selectedBook.IsbnNavigation.Titel}' from {selectedStore.ButikensNamn}.");
                Console.WriteLine($"New amount: {selectedBook.Antal}");
            }
        }
    }
}

