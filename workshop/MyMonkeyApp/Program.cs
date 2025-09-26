
using MyMonkeyApp;

class Program
{
	private static readonly string[] AsciiArts = new[]
	{
		@"  (o o)\n (  V  )\n/--m-m-",
		@"   w  c( .. )o   ( - )\n    (  -  )    (  - )\n    (  -  )    (  - )",
		@"   (o_/o)\n  (='.'=)\n  ("""")_("""")",
		@"   (o.o)\n   (> <)\n  (  :  )",
		@"   (o.o)\n  (  :  )\n  (  :  )"
	};

	static async Task Main()
	{
		while (true)
		{
			Console.Clear();
			PrintRandomAsciiArt();
			Console.WriteLine("==== Monkey App ====");
			Console.WriteLine("1. 모든 원숭이 나열");
			Console.WriteLine("2. 이름으로 특정 원숭이의 세부 정보 가져오기");
			Console.WriteLine("3. 무작위 원숭이 가져오기");
			Console.WriteLine("4. 앱 종료");
			Console.Write("메뉴를 선택하세요: ");
			var input = Console.ReadLine();
			Console.WriteLine();
			switch (input)
			{
				case "1":
					await ListAllMonkeys();
					break;
				case "2":
					await ShowMonkeyByName();
					break;
				case "3":
					await ShowRandomMonkey();
					break;
				case "4":
					Console.WriteLine("앱을 종료합니다.");
					return;
				default:
					Console.WriteLine("잘못된 입력입니다. 엔터를 눌러 계속하세요.");
					Console.ReadLine();
					break;
			}
		}
	}

	static void PrintRandomAsciiArt()
	{
		var rand = new Random();
		var art = AsciiArts[rand.Next(AsciiArts.Length)];
		Console.WriteLine(art);
		Console.WriteLine();
	}

	static async Task ListAllMonkeys()
	{
		var monkeys = await MonkeyHelper.GetMonkeysAsync();
		Console.WriteLine("이름\t\t서식지\t\t개체수");
		Console.WriteLine(new string('-', 40));
		foreach (var m in monkeys)
		{
			Console.WriteLine($"{m.Name}\t{m.Location}\t{m.Population}");
		}
		Console.WriteLine("\n엔터를 눌러 계속...");
		Console.ReadLine();
	}

	static async Task ShowMonkeyByName()
	{
		Console.Write("원숭이 이름을 입력하세요: ");
		var name = Console.ReadLine();
		var monkey = await MonkeyHelper.GetMonkeyByNameAsync(name ?? string.Empty);
		if (monkey == null)
		{
			Console.WriteLine("해당 이름의 원숭이를 찾을 수 없습니다.");
		}
		else
		{
			PrintMonkeyDetails(monkey);
		}
		Console.WriteLine("\n엔터를 눌러 계속...");
		Console.ReadLine();
	}

	static async Task ShowRandomMonkey()
	{
		var monkey = await MonkeyHelper.GetRandomMonkeyAsync();
		if (monkey == null)
		{
			Console.WriteLine("원숭이 데이터가 없습니다.");
		}
		else
		{
			PrintMonkeyDetails(monkey);
			var counts = MonkeyHelper.GetRandomAccessCounts();
			Console.WriteLine($"(무작위로 선택된 횟수: {counts[monkey.Name]})");
		}
		Console.WriteLine("\n엔터를 눌러 계속...");
		Console.ReadLine();
	}

	static void PrintMonkeyDetails(Monkey m)
	{
		Console.WriteLine($"이름: {m.Name}");
		Console.WriteLine($"서식지: {m.Location}");
		Console.WriteLine($"개체수: {m.Population}");
		Console.WriteLine($"설명: {m.Details}");
		if (!string.IsNullOrWhiteSpace(m.Image))
			Console.WriteLine($"이미지: {m.Image}");
		if (m.Latitude.HasValue && m.Longitude.HasValue)
			Console.WriteLine($"위치: {m.Latitude}, {m.Longitude}");
	}
}
