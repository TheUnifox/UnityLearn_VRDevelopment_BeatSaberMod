using System;
using System.Collections.Generic;

// Token: 0x02000071 RID: 113
public class ProfanityFilter
{
    // Token: 0x060004E4 RID: 1252 RVA: 0x0000D108 File Offset: 0x0000B308
    public ProfanityFilter(IEnumerable<string> wordList)
    {
        foreach (string text in wordList)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this._trie.AddWord(text, 0);
            }
        }
    }

    // Token: 0x060004E5 RID: 1253 RVA: 0x0000D170 File Offset: 0x0000B370
    public bool IsProfane(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (this._trie.IsMatch(word, i))
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x060004E6 RID: 1254 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
    private static IEnumerable<char> GetLookalikeLetters(char c)
    {
        yield return c;
        if (c == 'L')
        {
            yield return 'I';
        }
        if (c == 'I')
        {
            yield return 'L';
        }
        yield break;
    }

    // Token: 0x060004E7 RID: 1255 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
    private static char GetLeetEquivalent(char c)
    {
        switch (c)
        {
            case '0':
                return 'O';
            case '1':
                return 'I';
            case '2':
                return 'Z';
            case '3':
                return 'E';
            case '4':
                return 'A';
            case '5':
                return 'S';
            case '6':
                return 'G';
            case '7':
                return 'T';
            case '8':
                return 'B';
            case '9':
                return 'G';
            default:
                return c;
        }
    }

    // Token: 0x040001DF RID: 479
    private readonly ProfanityFilter.TrieNode _trie = new ProfanityFilter.TrieNode();

    // Token: 0x02000149 RID: 329
    private class TrieNode
    {
        // Token: 0x06000848 RID: 2120 RVA: 0x0001585C File Offset: 0x00013A5C
        public void AddWord(string word, int index)
        {
            this._shortestWord = Math.Min(this._shortestWord, word.Length - index);
            if (index == word.Length)
            {
                return;
            }
            if (this._children == null)
            {
                this._children = new Dictionary<char, ProfanityFilter.TrieNode>();
            }
            foreach (char key in ProfanityFilter.GetLookalikeLetters(char.ToUpperInvariant(word[index])))
            {
                ProfanityFilter.TrieNode trieNode;
                if (!this._children.TryGetValue(key, out trieNode))
                {
                    trieNode = new ProfanityFilter.TrieNode();
                    this._children[key] = trieNode;
                }
                trieNode.AddWord(word, index + 1);
            }
        }

        // Token: 0x06000849 RID: 2121 RVA: 0x00015910 File Offset: 0x00013B10
        public bool IsMatch(string word, int index)
        {
            if (word.Length - index < this._shortestWord)
            {
                return false;
            }
            if (word.Length == index)
            {
                return true;
            }
            if (this._children == null)
            {
                return true;
            }
            char c = char.ToUpperInvariant(word[index]);
            c = ProfanityFilter.GetLeetEquivalent(c);
            ProfanityFilter.TrieNode trieNode;
            return this._children.TryGetValue(c, out trieNode) && trieNode.IsMatch(word, index + 1);
        }

        // Token: 0x0400043F RID: 1087
        private Dictionary<char, ProfanityFilter.TrieNode> _children;

        // Token: 0x04000440 RID: 1088
        private int _shortestWord = int.MaxValue;
    }
}
