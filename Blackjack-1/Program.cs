using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

static void Judge(Player player, Dealer dealer)
{
    int p = player.SumPScore();
    int d = dealer.SumDScore();

    if (p > 21)
        Console.WriteLine("플레이어 버스트! 플레이어 패배");
    else if (d > 21)
        Console.WriteLine("딜러 버스트! 플레이어 승리");
    else if (p > d)
        Console.WriteLine("플레이어 승리!");
    else if (p < d)
        Console.WriteLine("딜러 승리!");
    else
        Console.WriteLine("무승부");
}
//Card card = new Card();
Player player = new Player();
Dealer dealer = new Dealer();


Console.WriteLine("=== 블랙잭 게임 ===");
Console.WriteLine();
Card.cardShuffle();
Console.WriteLine("=== 초기 패 ===\n");
dealer.FirstDeck();
player.FirstDeck();
player.TurnPlayer();

if (player.SumPScore() == 21)
{
    Console.WriteLine("블랙잭! 플레이어 승리!");
}
else if (player.SumPScore() <= 21)
{
    dealer.DealerTurn();
}

Judge(player, dealer);

static class Card
{
    static int index1 = 0;
    static int index2 = 0;

    public static StringBuilder[,] cardImoge = new StringBuilder[4, 13];

    static Card()
    {
        for (int i = 0; i < 4; i++)
        {

            for (int j = 0; j < 13; j++)
            {
                cardImoge[i, j] = new StringBuilder();
                if (i == 0)
                {
                    cardImoge[i, j].Append("◆");
                }
                else if (i == 1)
                {
                    cardImoge[i, j].Append("♥");
                }
                else if (i == 2)
                {
                    cardImoge[i, j].Append("♣");
                }
                else if (i == 3)
                {
                    cardImoge[i, j].Append("♠");
                }
                if (0 < j && j < 10)
                {
                    cardImoge[i, j].Append($"{j + 1}");
                }
                else if (j == 0)
                {
                    cardImoge[i, j].Append($"A");
                }

                else if (j == 10)
                {
                    cardImoge[i, j].Append($"J");
                }

                else if (j == 11)
                {
                    cardImoge[i, j].Append($"Q");
                }
                else if (j == 12)
                {
                    cardImoge[i, j].Append($"K");
                }
            }

        }
        cardImoge.ToString();
    }

    public static string cardDraw()
    {
        string str = cardImoge[index1, index2].ToString();
        index2++;
        if (index2 == 12)
            index1++;
        return str;
    }
    public static void cardShuffle() //Clear
    {
        Random r = new Random();
        int total = cardImoge.Length;
        for (int i = total - 1; i > 0; i--)
        {
            int j = r.Next(i + 1);
            int x1 = i / 13;
            int y1 = i % 13;
            int x2 = j / 13;
            int y2 = j % 13;
            StringBuilder temp = cardImoge[x1, y1];
            cardImoge[x1, y1] = cardImoge[x2, y2];
            cardImoge[x2, y2] = temp;

        }
        Console.WriteLine("카드 섞는중 ....\n");
    }


}

class Player
{
    int i = 2;
    private string[] pcard = new string[8];
    public int pscore = 0;
    public bool pturn = true;

    public Player()
    {

    }
    public void FirstDeck()
    {
        pcard[0] = Card.cardDraw();
        pcard[1] = Card.cardDraw();
        ShowPcard();
    }

    public void AddPcard(string pcards)
    {
        Console.WriteLine($"플레이어가 카드를 받았습니다: {pcards}");
        pcard[i] = pcards;
        i++;
    }
    public void ShowPcard()
    {
        Console.Write($"플레이어의 패:");
        for (int i = 0; i < pcard.Length; i++)
        {
            if (pcard[i] != null)
            {
                Console.Write($"[{pcard[i]}] ");
            }

        }
        Console.WriteLine();

        if (pscore > 21)
        {
            Console.WriteLine($"{SumPScore()}점! 버스트...\n");
        }
        else
        {
            Console.WriteLine($"플레이어의 점수: {SumPScore()}\n");
        }



    }
    public int SumPScore()
    {
        int a = 0; ;
        pscore = 0;
        for (int i = 0; i < pcard.Length; i++)
        {
            if (pcard[i] != null)
            {
                string str = pcard[i].Substring(1);
                if (int.TryParse(str, out int result))
                {
                    pscore += result;
                }
                else
                {
                    switch (str)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            pscore += 10;
                            break;
                        case "A":
                            a++;
                            break;
                    }
                }
            }
        }
        for (int i = 0; i < a; i++)
        {
            if (pscore < 11)
            {
                pscore += 11;
            }
            else
            {
                pscore += 1;
            }
        }
        return pscore;
    }

    public void TurnPlayer()
    {
        while (true)
        {
            Console.Write("H(Hit) 또는 S(Stand)를 선택하세요: ");
            string turn = Console.ReadLine();

            if (turn == "H")
            {
                AddPcard(Card.cardDraw());
                ShowPcard();

                int pscore = SumPScore();

                if (pscore > 21)
                {
                    Console.WriteLine($"{pscore}점! 버스트...\n");
                    break;
                }
            }
            else if (turn == "S")
            {
                Console.WriteLine("턴을 종료합니다.\n");
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public void ShowResult()
    {

    }

}

class Dealer
{
    private string[] dcard = new string[8];
    public int dscore = 0;
    int i = 2;
    public Dealer()
    {

    }

    public void FirstDeck()
    {
        dcard[0] = Card.cardDraw();
        dcard[1] = Card.cardDraw();
        ShowDcard();

    }

    public void AddDcard(string dcards)
    {
        Console.WriteLine($"딜러가 카드를 받았습니다: {dcards}");
        dcard[i] = dcards;
        i++;
    }

    public void ShowDcard()
    {

        Console.Write($"딜러의 패: ");
        for (int i = 0; i < dcard.Length; i++)
        {
            if (i == 0)
            {
                Console.Write("[??] ");
            }
            else if (dcard[i] != null)
            {
                Console.Write($"[{dcard[i]}] ");
            }
        }
        Console.WriteLine();

    }
    public void DealerTurn()
    {
        Console.WriteLine("\n딜러 턴 시작");
        ShowDcard();

        while (SumDScore() < 17)
        {
            AddDcard(Card.cardDraw());
            ShowDcard();
        }

        if (SumDScore() > 21)
        {
            Console.WriteLine($"{SumDScore()}점! 딜러 버스트!");
        }
        else
        {
            Console.WriteLine($"딜러 스탠드: {SumDScore()}점");
        }
    }

    public int SumDScore()
    {
        int a = 0;
        dscore = 0;

        for (int i = 0; i < dcard.Length; i++)
        {
            if (dcard[i] != null)
            {
                string str = dcard[i].Substring(1);

                if (int.TryParse(str, out int result))
                {
                    dscore += result;
                }
                else
                {
                    switch (str)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            dscore += 10;
                            break;
                        case "A":
                            a++;
                            break;
                    }
                }
            }
        }


        for (int i = 0; i < a; i++)
        {
            if (dscore < 11)
                dscore += 11;
            else
                dscore += 1;
        }

        return dscore;
    }
}


