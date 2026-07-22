using System;
using System.Collections.Generic;

namespace A_star_arama
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid();
            var cozum = AStar.AStarCoz(grid, grid.TahminiMesafe);
            grid.RotayıYazdır(cozum);
            Console.WriteLine("\nBulunan Rota:"); //buldugu rotayı yazdırma.
            if (cozum != null)
            {
                grid.RotayıYazdır(cozum);
                Console.WriteLine("\nRota Koordinatları:");
                foreach (var node in cozum)
                {
                    Console.Write($"({node.Item1},{node.Item2}) -> ");
                }
            }
            else
            {
                Console.WriteLine("Yol bulunamadı!");
            }
            Console.ReadKey();
        }
    }
    public class PriorityQueue<TElement, TPriority>
    {
        private readonly SortedList<TPriority, Queue<TElement>> _elements = new SortedList<TPriority, Queue<TElement>>();

        public int Count { get; private set; }

        public void Enqueue(TElement item, TPriority priority)
        {
            if (!_elements.ContainsKey(priority))
                _elements[priority] = new Queue<TElement>();

            _elements[priority].Enqueue(item);
            Count++;
        }

        public TElement Dequeue()
        {
            var first = _elements.Keys[0];
            var item = _elements[first].Dequeue();

            if (_elements[first].Count == 0)
                _elements.Remove(first);

            Count--;
            return item;
        }
    } //PriorityQueue sınıfı. 

    public class Grid
    {
        public int En { get; }
        public int Boy { get; }
        //koordinatları tuple'da tutmayı tercih ettim çünkü sonradan değiştirilemezler ve örneğin Start.Item1 diyerek direkt olarak x koordinatına erişebiliyorum.
        public Tuple<int, int> Start { get; } 
        public Tuple<int, int> Goal { get; }
        private char[,] _labirent;
        public char[,] Labirent
        {
            get => _labirent;
            private set => _labirent = value ?? new char[Boy, En];
        }

        private Dictionary<char, int> AgirlikDegerleri = new Dictionary<char, int>() //Ağırlık değerleri
        {
            {'.', 1}, {'S', 0}, {'G', 0}, {'%', 100}
        };

        public Grid(int width = 16, int height = 16)
        {
            En = width;
            Boy = height;
            Start = Tuple.Create(0, 0);
            Goal = Tuple.Create(12, 15);
            Labirent = LabirentOluştur(); // Maze burada başlatılıyor
        }
        public void RotayıYazdır(List<Tuple<int, int>> yol) //S'den G'ye giden rotayı yazdırmak için.
        {
            if (Labirent == null || yol == null) return;

            for (int i = 0; i < Boy; i++)
            {
                for (int j = 0; j < En; j++)
                {
                    var mevcutYer = Tuple.Create(i, j); // Labirentteki anlık pozisyon bilgisi (x,y)

                    if (mevcutYer.Equals(Start))
                    {
                        Console.Write("S ");
                    }
                    else if (mevcutYer.Equals(Goal))
                    {
                        Console.Write("G ");
                    }
                    else if (yol.Contains(mevcutYer))
                    {
                        Console.Write("* ");
                    }
                    else
                    {
                        Console.Write(Labirent[i, j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        private char[,] LabirentOluştur()
        {
            char[,] maze = new char[Boy, En];
            Random random = new Random();

            
            int maxAttempts = 100;
            int attempt = 0;

            do
            {
                attempt++;
                if (attempt >= maxAttempts) //Uygulamayı çalıştırınca sonsuz döngüyü önlemek için maksimum deneme sayısı.
                {
                    Console.WriteLine("Max deneme sayısına ulaşıldı!");
                    return maze; // Boş labirent döndür
                }

                // Gidilebilire yolları labirente yerleştirme.
                for (int i = 0; i < Boy; i++)
                    for (int j = 0; j < En; j++)
                        maze[i, j] = '.';

                // Engelleri rastgele %25 oranında yerleştirme. Engeller rastgele oluşturulduğu için bazen rota bulunamayabilir.
                for (int k = 0; k < En * Boy * 0.25; k++)
                {
                    int x, y;
                    do
                    {
                        x = random.Next(En);
                        y = random.Next(Boy);
                    } while ((y == Start.Item1 && x == Start.Item2) ||
                            (y == Goal.Item1 && x == Goal.Item2));

                    maze[y, x] = '%';
                }

                maze[Start.Item1, Start.Item2] = 'S';
                maze[Goal.Item1, Goal.Item2] = 'G';

            } while (!YolVarMi()); // Yol bulunana kadar döngüyü sürdür. 

            return maze;
        }

        public List<Tuple<int, int>> KomsuDugumleriBul(Tuple<int, int> dugum)
        {
            var komsular = new List<Tuple<int, int>>();
            if (Labirent == null) return komsular; // Labirent henüz oluşmadıysa boş liste döndür

            var yönler = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach (var (dy, dx) in yönler)
            {
                int y = dugum.Item1 + dy; // Yeni satır ve sütun pozisyonu
                int x = dugum.Item2 + dx;

                // Labirent sınırları içinde mi?
                if (y >= 0 && y < Boy && x >= 0 && x < En && Labirent[y, x] != '%')
                {
                    komsular.Add(Tuple.Create(y, x));
                }
            }
            return komsular;
        }

        public int TahminiMesafe(Tuple<int, int> node) =>
            Math.Abs(node.Item1 - Goal.Item1) + Math.Abs(node.Item2 - Goal.Item2); // Manhattan mesafesini hesaplar (dikey+yatay uzaklık toplamı)

        public int MaliyetAl(Tuple<int, int> node) =>
            AgirlikDegerleri.TryGetValue(Labirent[node.Item1, node.Item2], out int cost) ? cost : 1; // Karakter tipine göre maliyet değerini döndürür (varsayılan=1)

        public void LabirentiYazdır()
        {
            if (Labirent == null) return;

            for (int i = 0; i < Boy; i++)
            {
                for (int j = 0; j < En; j++)
                {
                    Console.Write(Labirent[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool YolVarMi()
        {
            return AStar.AStarCoz(this, TahminiMesafe) != null;
        }
    }

    public class AStar
    {
        public static List<Tuple<int, int>> AStarCoz(Grid problem, Func<Tuple<int, int>, int> h)
        {
            var closed = new HashSet<Tuple<int, int>>();
            // Öncelik kuyruğu (f = g + w*h değerine göre sıralı)
            var open = new PriorityQueue<Tuple<int, int>, int>();

            // Düğüm bilgilerini tutacak sözlükler
            var gValues = new Dictionary<Tuple<int, int>, int>();
            var parent = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

            // Başlangıç düğümünü ekle (f = g + w*h, g=0 olduğu için f = w*h)
            const int w = 1; // Ağırlık katsayısı
            open.Enqueue(problem.Start, w * h(problem.Start));
            gValues[problem.Start] = 0;

            while (true)
            {
                // Açık liste boşsa başarısız
                if (open.Count == 0) return null;

                // En düşük f değerli düğümü al
                var node = open.Dequeue();

                // Hedefe ulaşıldıysa çözümü döndür
                if (node.Equals(problem.Goal))
                {
                    return YoluOlustur(parent, node);
                }

                // Zaten işlenmişse atla
                if (closed.Contains(node)) continue;

                // Kapalı listeye ekle
                closed.Add(node);

                // Komşuları genişlet
                foreach (var child in Genislet(node, open, closed, problem))
                {
                    // Yeni g değerini hesapla
                    int tentativeG = gValues[node] + 1;

                    // Eğer daha iyi bir yol bulunduysa
                    if (!gValues.ContainsKey(child) || tentativeG < gValues[child])
                    {
                        parent[child] = node;
                        gValues[child] = tentativeG;
                        int fValue = tentativeG + w * h(child);
                        open.Enqueue(child, fValue);
                    }
                }
            }
        }

        private static List<Tuple<int, int>> Genislet(
            Tuple<int, int> node,
            PriorityQueue<Tuple<int, int>, int> open,
            HashSet<Tuple<int, int>> closed,
            Grid grid)
        {
            var children = new List<Tuple<int, int>>();

            // Null kontrolü (Labirent oluşmadan genişlet fonksiyonu çalışıyordu o yüzden ekledim.)
            if (grid?.Labirent == null || grid.Boy <= 0 || grid.En <= 0)
                return children;

            var yonler = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

            foreach (var (dy, dx) in yonler)
            {
                int y = node.Item1 + dy;
                int x = node.Item2 + dx;

                // Sınır kontrolleri
                if (y < 0 || y >= grid.Boy || x < 0 || x >= grid.En)
                    continue;

                // Labirent oluşturmadan önceski ek kontrol
                if (y >= grid.Labirent.GetLength(0) || x >= grid.Labirent.GetLength(1))
                    continue;

                var child = Tuple.Create(y, x);

                if (grid.Labirent[y, x] == '%')
                    continue;

                if (closed.Contains(child))
                    continue;

                children.Add(child);
            }
            return children;
        }

        private static List<Tuple<int, int>> YoluOlustur(
            Dictionary<Tuple<int, int>, Tuple<int, int>> parent,
            Tuple<int, int> hedef)
        {
            var path = new List<Tuple<int, int>> { hedef };
            while (parent.ContainsKey(hedef))
            {
                hedef = parent[hedef];
                path.Insert(0, hedef);
            }
            return path;
        }
    }
}