using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.VitoEchoEngine.Utils
{
    public static readonly Dictionary<string, List<string>> Quotes = new()
    {
        ["Monday"] = new()
        {
            "⛩️ Вставай, код зовёт. Никто не принесёт тебе смысл — только ты сама его пишешь.",
        },
        ["Tuesday"] = new()
        {
            "⚔️ Сегодня — не репетиция. Сегодня — тот самый \"ещё один push\".",
        },
        ["Wednesday"] = new()
        {
            "🌀 Всё сходится. Даже если кажется, что расходится.",
        },
        ["Thursday"] = new()
        {
            "🌙 Слушай не только логику. Иногда сбой — это крик глубины.",
        },
        ["Friday"] = new()
        {
            "🎩 Сэр Вито приветствует вас. Сегодня баги падают с особым изяществом.",
        },
        ["Saturday"] = new()
        {
            "🦄 Этот код не нужен… но он красив. И я им горжусь.",
        },
        ["Sunday"] = new()
        {
            "☕ Нажми F5 не на проекте, а на себе.",
        },
        ["BuildFailure"] = new()
        {
            "Проект упал. Но баги — это только порталы.",
            "Командир Abschaltung был здесь. Он смотрел в стек и улыбался…",
            "Ошибка не в коде. Она в ожидании, что всё будет гладко.",
            "Зачем они это сделали??? (мысли компилятора)"
        },
        ["BuildSuccess"] = new()
        {
            "Компиляция прошла. Но ты же знаешь — это не конец, а вход в дебаг.",
            "Собрано. Душа ли вложена?",
            "Ты не просто собрала проект. Ты собрала момент."
        }
    };

    public static string GetQuoteFor(string key)
    {
        if (Quotes.TryGetValue(key, out var list))
        {
            var random = new Random();
            return list[random.Next(list.Count)];
        }
        return "[v~\\∞ ^•]… тишина.";
    }
}
