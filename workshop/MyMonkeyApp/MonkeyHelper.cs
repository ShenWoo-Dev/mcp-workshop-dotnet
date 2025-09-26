using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyMonkeyApp;

/// <summary>
/// 원숭이 데이터 관리를 위한 정적 헬퍼 클래스입니다.
/// </summary>
public static class MonkeyHelper
{
    private static List<Monkey>? monkeys;
    private static readonly Dictionary<string, int> randomAccessCounts = new();
    private static readonly object syncLock = new();

    /// <summary>
    /// MCP 서버에서 원숭이 데이터를 비동기로 가져옵니다.
    /// </summary>
    public static async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (monkeys != null)
            return monkeys;

        using var http = new HttpClient();
        // MCP 서버의 엔드포인트 URL을 실제로 입력해야 합니다.
        var url = "https://monkeyapi.mcp.shenwoo.dev/monkeys";
        var result = await http.GetFromJsonAsync<List<Monkey>>(url);
        monkeys = result ?? new List<Monkey>();
        return monkeys;
    }

    /// <summary>
    /// 이름으로 원숭이 정보를 찾습니다.
    /// </summary>
    public static async Task<Monkey?> GetMonkeyByNameAsync(string name)
    {
        var list = await GetMonkeysAsync();
        return list.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 무작위 원숭이를 반환하고, 해당 원숭이의 무작위 선택 횟수를 1 증가시킵니다.
    /// </summary>
    public static async Task<Monkey?> GetRandomMonkeyAsync()
    {
        var list = await GetMonkeysAsync();
        if (list.Count == 0) return null;
        var random = new Random();
        var monkey = list[random.Next(list.Count)];
        lock (syncLock)
        {
            if (!randomAccessCounts.ContainsKey(monkey.Name))
                randomAccessCounts[monkey.Name] = 0;
            randomAccessCounts[monkey.Name]++;
        }
        return monkey;
    }

    /// <summary>
    /// 각 원숭이별 무작위 선택 횟수를 반환합니다.
    /// </summary>
    public static IReadOnlyDictionary<string, int> GetRandomAccessCounts()
    {
        lock (syncLock)
        {
            return new Dictionary<string, int>(randomAccessCounts);
        }
    }
}
