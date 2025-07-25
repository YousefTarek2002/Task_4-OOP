namespace Task_4_OOP
{
    public class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", string password = "1234", double balance = 0.0)
        {
            Name = name;
            Password = password;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount <= 0) return false;
            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(double amount)
        {
            if (amount <= 0 || Balance < amount) return false;
            Balance -= amount;
            return true;
        }

        public virtual void Save()
        {
            Console.WriteLine($"[Saved] Account: {Name}, Balance: {Balance:F2}");
        }

        public override string ToString()
        {
            return $"{Name} (General): {Balance:F2}";
        }

        public static double operator +(Account a1, Account a2)
        {
            return a1.Balance + a2.Balance;
        }
    }

    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string name = "Unnamed Savings", string password = "1234", double balance = 0.0, double interestRate = 3.0)
            : base(name, password, balance)
        {
            InterestRate = interestRate;
        }

        public override bool Deposit(double amount)
        {
            if (amount <= 0) return false;
            double interest = amount * (InterestRate / 100);
            return base.Deposit(amount + interest);
        }

        public override void Save()
        {
            Console.WriteLine($"[Saved] Savings: {Name}, Balance: {Balance:F2}, Interest: {InterestRate}%");
        }

        public override string ToString()
        {
            return $"{Name} (Savings): {Balance:F2}, Interest: {InterestRate}%";
        }
    }

    public class CheckingAccount : Account
    {
        private const double Fee = 1.5;

        public CheckingAccount(string name = "Unnamed Checking", string password = "1234", double balance = 0.0)
            : base(name, password, balance) { }

        public override bool Withdraw(double amount)
        {
            return base.Withdraw(amount + Fee);
        }

        public override void Save()
        {
            Console.WriteLine($"[Saved] Checking: {Name}, Balance: {Balance:F2}");
        }

        public override string ToString()
        {
            return $"{Name} (Checking): {Balance:F2}";
        }
    }

    public class TrustAccount : SavingsAccount
    {
        private int withdrawals = 0;
        private const int MaxWithdrawals = 3;
        private const double BonusThreshold = 5000.0;
        private const double Bonus = 50.0;

        public TrustAccount(string name = "Unnamed Trust", string password = "1234", double balance = 0.0, double interestRate = 3.0)
            : base(name, password, balance, interestRate) { }

        public override bool Deposit(double amount)
        {
            if (amount >= BonusThreshold)
                amount += Bonus;
            return base.Deposit(amount);
        }

        public override bool Withdraw(double amount)
        {
            if (withdrawals >= MaxWithdrawals || amount > Balance * 0.2)
                return false;

            bool result = base.Withdraw(amount);
            if (result) withdrawals++;
            return result;
        }

        public override void Save()
        {
            Console.WriteLine($"[Saved] Trust: {Name}, Balance: {Balance:F2}, Withdrawals left: {3 - withdrawals}");
        }

        public override string ToString()
        {
            return $"{Name} (Trust): {Balance:F2}, Interest: {InterestRate}%, Withdrawals left: {3 - withdrawals}";
        }
    }

    public static class AccountUtil
    {
        public static void Display(Account acc)
        {
            Console.WriteLine("\n=== Account Info ===");
            Console.WriteLine(acc);
        }

        public static void Deposit(Account acc, double amount)
        {
            Console.WriteLine($"\n--- Depositing {amount} ---");
            Console.WriteLine(acc.Deposit(amount) ? $"Deposited {amount} to {acc.Name}" : $"Failed to deposit to {acc.Name}");
        }

        public static void Withdraw(Account acc, double amount)
        {
            Console.WriteLine($"\n--- Withdrawing {amount} ---");
            Console.WriteLine(acc.Withdraw(amount) ? $"Withdrew {amount} from {acc.Name}": $"Failed to withdraw from {acc.Name}");
        }

        public static void Save(Account acc)
        {
            Console.WriteLine("\n--- Saving Account ---");
            acc.Save();
        }

        public static void Transfer(Account Sender, Account Resever, double amount)
        {
            Console.WriteLine($"\n--- Transferring {amount} from {Sender.Name} to {Resever.Name} ---");
            if (Sender.Withdraw(amount))
            {
                Resever.Deposit(amount);
                Console.WriteLine("Transfer successful.");
            }
            else
            {
                Console.WriteLine("Transfer failed: insufficient funds.");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            List<Account> accounts = new List<Account>
            {
                new SavingsAccount("Yousef", "0000", 3000, 4.5),
                new CheckingAccount("Hana", "1111", 1500),
                new TrustAccount("Kareem", "2222", 8000, 5.0),
                new Account("Basic", "3333", 1000)
            };

            Console.WriteLine("=== Login ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Account currentUser = accounts.Find(a => a.Name == username && a.Password == password);
            if (currentUser == null)
            {
                Console.WriteLine("Login failed. Exiting...");
                return;
            }

            Console.WriteLine($"Welcome, {currentUser.Name}!");

            int choice;
            do
            {
                Console.WriteLine("\n=== Bank Menu ===");
                Console.WriteLine("1. Show account");
                Console.WriteLine("2. Deposit to your account");
                Console.WriteLine("3. Withdraw from your account");
                Console.WriteLine("4. Save your account");
                Console.WriteLine("5. Transfer to another account");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice)) continue;

                switch (choice)
                {
                    case 1:
                        AccountUtil.Display(currentUser);
                        break;
                    case 2:
                        Console.Write("Enter deposit amount: ");
                        if (double.TryParse(Console.ReadLine(), out double dAmt))
                            AccountUtil.Deposit(currentUser, dAmt);
                        break;
                    case 3:
                        Console.Write("Enter withdrawal amount: ");
                        if (double.TryParse(Console.ReadLine(), out double wAmt))
                            AccountUtil.Withdraw(currentUser, wAmt);
                        break;
                    case 4:
                        AccountUtil.Save(currentUser);
                        break;
                    case 5:
                        Console.Write("Enter recipient's name: ");
                        string recipientName = Console.ReadLine();
                        Account recipient = accounts.Find(a => a.Name == recipientName);
                        if (recipient == null || recipient == currentUser)
                        {
                            Console.WriteLine("Invalid recipient.");
                            break;
                        }
                        Console.Write("Enter transfer amount: ");
                        if (double.TryParse(Console.ReadLine(), out double tAmt))
                            AccountUtil.Transfer(currentUser, recipient, tAmt);
                        break;
                    case 0:
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            } while (choice != 0);
        }
    }
}
