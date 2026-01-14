using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using wpfcraft.ChunkProviders;
using wpfcraft.Animations;
using wpfcraft.Gui;
using wpfcraft.Items.Containers;
using wpfcraft.Items;
using wpfcraft.PlayerData;
using wpfcraft.Blocks;
using wpfcraft.Gui.Guis;

namespace wpfcraft
{
    public partial class MainWindow : Window
    {
        public long Rendered = 0;
        public long RenderedTotal = 0;
        public int FrameLoop = 0;
        public string Ver = "0.4.0";
        public Player Player;
        Stopwatch Timer = new();
        Stopwatch FramerateTimer = new();
        Random Rand = new();
        Process TheProcess = Process.GetCurrentProcess();
        Server Server;
        Server InternalServer;
        ChunkProvider ChunkProvider;
        GuiIngame GuiIngame;
        event EventHandler BlockTypeSelectionChanged;

        public MainWindow()
        {
            InitializeComponent();
            Start();
            ChunkProvider = new(this);
            CompositionTarget.Rendering += OnRender;
            MouseWheel += OnScroll;
            this.world.RenderTransform = new ScaleTransform(48, 48);
            double scale = this.Width * 0.00125;
            this.SizeChanged += ScaleWindow;
            this.Title = $"WPFCraft {Ver}";
            int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)SystemParameters.PrimaryScreenHeight;
            this.fsOptionText.Content = $"This will render WPFCraft at {screenWidth}x{screenHeight}.";
            foreach (UIElement e in uiFSOption.Children)
            {
                if (e is Button)
                {
                    Button btn = (Button)e;
                    btn.Click += HandleButton;
                }
            }
            foreach (UIElement e in uiMultiplayer.Children)
            {
                if (e is Button)
                {
                    Button btn = (Button)e;
                    btn.Click += HandleButton;
                }
            }
            foreach (Canvas c in windowGrid.Children)
            {
                c.RenderTransform = new ScaleTransform(scale, scale);
            }
        }

        private void HandleButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == fsOptionYesBtn)
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
                int screenHeight = (int)SystemParameters.PrimaryScreenHeight;
                double scale = screenWidth * 0.00125;
                foreach (Canvas c in windowGrid.Children)
                {
                    c.RenderTransform = new ScaleTransform(scale, scale);
                }
                uiFSOption.Visibility = Visibility.Hidden;
            }
            else if (btn == joinBtn)
            {
                string[] content = ipField.Text.Split(':');
                this.JoinServer(content[0], Convert.ToInt32(content[1]));
            }
            else if (btn == openBtn)
            {
                string[] content = ipField.Text.Split(':');
                this.OpenServer(content[0], Convert.ToInt32(content[1]));
            }
            else
            {
                uiFSOption.Visibility = Visibility.Hidden;
            }
        }

        private void ScaleWindow(object sender, SizeChangedEventArgs e)
        {
            double scale = this.Width * 0.00125;
            foreach (Canvas c in windowGrid.Children)
            {
                c.RenderTransform = new ScaleTransform(scale, scale);
            }
        }

        private void OnRender(object? sender, EventArgs e)
        {
            ++Rendered;
            ++RenderedTotal;
            if (!Timer.IsRunning)
            {
                Timer.Start();
            }
            if (!FramerateTimer.IsRunning)
            {
                FramerateTimer.Start();
            }
            if (this.Server != null)
            {
                string s = "";
                foreach (string[] playerData in this.Server.PlayersToClient)
                {
                    s = s + $"{playerData[0]} ({playerData[1]})\n";
                }
                this.networkInfo.Content = $"Network\nConnected: {this.Server.Client.Connected}\nPlayers: {this.Server.PlayersToClient.Count}\n\n{s}";
            }
            if (FramerateTimer.ElapsedMilliseconds >= 1000)
            {
                this.TheProcess = Process.GetCurrentProcess();
                if (TheProcess.PrivateMemorySize64 / (1024 * 1024) > 1000)
                {
                    Stop(null, "Memory usage has exceeded the limit of 1000 MB.", "My programming skills");
                }
                onScreenText.Content = $"WPFCraft {Ver} ({Rendered} fps, {TheProcess.PrivateMemorySize64 / (1024 * 1024)} MB of memory used)";
                Rendered = 0;
                UnloadOldestChunk();
                AskNewChunk();
                FramerateTimer.Restart();
                if (this.Server != null)
                {
                    foreach (string[] playerData in this.Server.PlayersToClient)
                    {
                        string name = playerData[0];
                        ulong id = Convert.ToUInt64(playerData[1]);
                        double x = Convert.ToDouble(playerData[2]);
                        double y = Convert.ToDouble(playerData[3]);
                        foreach(Player p in this.entities.Children)
                        {
                            if (p.Id != id)
                            {
                                if (id != this.Player.Id)
                                {
                                    Player player = new Player(name, id);
                                    player.SetPos(x, y);
                                    this.entities.Children.Add(player);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (Timer.ElapsedMilliseconds >= 10)
            {
                this.Player.PrevY = this.Player.Y;
                this.GuiIngame.Update(Player);
                ++FrameLoop;
                playerInfo.Content = $"Player:\n{this.Player.Name}\n{this.Player.X}\n{this.Player.Y}\n{this.Player.InitCalls}\nIsJumping: {this.Player.IsJumping}\nIsFalling: {this.Player.IsFalling}";
                if (Keyboard.IsKeyDown(Key.A))
                {
                    this.Player.SetX(this.Player.X -= 0.1);
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    this.Player.SetX(this.Player.X += 0.1);
                }
                if (Keyboard.IsKeyDown(Key.Space))
                {
                    if (!this.Player.IsJumping && !this.Player.IsFalling)
                    {
                        this.Player.MovementY = 0.075;
                        this.Player.StartJumpY = Player.Y;
                        this.Player.IsJumping = true;
                    }
                }
                if (FrameLoop >= 100)
                {
                    FrameLoop = 0;
                }
                camera.ScrollToHorizontalOffset(Player.X * 48 - 400 + Player.Width / 2 * 48);
                camera.ScrollToVerticalOffset(Player.Y * 48 - 200 + Player.Height / 2 * 48);
                HandlePlayerMovement(this.Player);
                this.Player.PrevY = Player.Y;
                Timer.Restart();
            }
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.F6))
            {
                this.Player.Inventory.AddItemToHotbar(new ItemBuildingBlock(10000), 8, 10);
            }
            if (Keyboard.IsKeyDown(Key.F5))
            {
                this.Player.SetX(1073741824);
            }
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                if (this.uiMultiplayer.Visibility == Visibility.Visible)
                {
                    this.uiMultiplayer.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.uiMultiplayer.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnScroll(object sender, MouseWheelEventArgs e)
        {
            if (GuiIngame != null)
            {
                if (e.Delta > 0)
                {
                    ++GuiIngame.Hotbar.Selection;
                }
                if (e.Delta < 0)
                {
                    --GuiIngame.Hotbar.Selection;
                }
            }
        }

        void Start()
        {
            CreatePlayer($"Player{Rand.Next(0, 1000)}", (ulong)Rand.NextInt64());
            GuiIngame GuiIngame = new GuiIngame(this);
            this.GuiIngame = GuiIngame;
            windowGrid.Children.Add(this.GuiIngame);
        }

        void HandlePlayerMovement(Player player)
        {
            player.CollisionX = player.X + player.Width / 2;
            if (player.PrevX != player.X && !player.IsJumping)
            {
                player.IsFalling = true;
                foreach (Chunk chunk in chunks.Children)
                {
                    double chunkX = Canvas.GetLeft(chunk);
                    if (player.CollisionX >= chunkX && player.CollisionX < chunkX + 16)
                    {
                        foreach (Block block in chunk.Children)
                        {
                            if (block.Id != 6 && block.Y < player.Y + 2 && block.Y > player.Y - 2)
                            {
                                Rect r1 = new Rect(player.X, player.Y, player.Width, player.Height);
                                Rect r2 = new Rect(block.X + chunkX, block.Y + 0.1, block.Width, block.Height - 0.1);
                                if (r1.IntersectsWith(r2))
                                {
                                    player.SetX(player.PrevX);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (this.Server != null)
                {
                    this.Server.Client.Client.Send(this.Server.BuildPacketPlayerPos(100, $"{this.Player.Id}:{this.Player.X}:{this.Player.Y}"));
                }
            }
            player.PrevX = this.Player.X;
            player.PrevY = this.Player.Y;
            if (player.Y > 127)
            {
                player.SetY(50);
            }
            if (player.IsFalling)
            {
                player.MovementY += 0.002;
                player.SetY(player.Y += player.MovementY);
                foreach (Canvas chunk in chunks.Children)
                {
                    double chunkX = Canvas.GetLeft(chunk);
                    if (player.CollisionX >= chunkX && player.CollisionX < chunkX + 16)
                    {
                        foreach (Block block in chunk.Children)
                        {
                            if (block.Id != 6)
                            {
                                Rect r1 = new Rect(player.X, player.Y, player.Width, player.Height);
                                Rect r2 = new Rect(block.X + chunkX, block.Y, block.Width, block.Height);
                                if (r1.IntersectsWith(r2))
                                {
                                    player.IsFalling = false;
                                    player.SetY(block.Y - player.Height);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (player.IsJumping)
            {
                player.MovementY -= 0.002;
                if (player.MovementY <= 0.01)
                {
                    player.IsJumping = false;
                    player.IsFalling = true;
                }
                player.SetY(player.Y -= player.MovementY);
            }
            else
            {
                player.IsFalling = false;
                player.IsJumping = false;
                player.MovementY = 0;
            }
        }

        public void CreatePlayer(string name, ulong id)
        {
            Player player = new Player(name, id);
            this.Player = player;
            Canvas.SetLeft(this.Player, this.Player.X);
            Canvas.SetTop(this.Player, this.Player.Y);
            entities.Children.Add(this.Player);
        }

        void BlockTouched(Block block, int type, MouseEventArgs e)
        {
            bool isLeftClick = false;
            switch (e.LeftButton)
            {
                case MouseButtonState.Pressed:
                    isLeftClick = true;
                    break;
            }
            Chunk blockChunk = (Chunk)block.Parent;
            double chunkX = Canvas.GetLeft(blockChunk);
            double blockX = block.X + chunkX;
            double x = blockX - Player.X;
            double y = block.Y - Player.Y;
            double radius = Math.Sqrt(x * x + y * y);
            switch (type)
            {
                case 0:
                    {
                        if (radius < 4)
                        {
                            if (block.Id == 6 && Player.Selection.PrevBlockId != 6)
                            {
                                AnimationSelectionFade anim = new();
                                BlockTypeSelectionChanged += (sender, EventArgs) => anim.Stop(this, EventArgs);
                                selectionBox.BeginAnimation(OpacityProperty, anim);
                            }
                            else if (block.Id != Player.Selection.PrevBlockId)
                            {
                                BlockTypeSelectionChanged?.Invoke(this, e);
                            }
                            Player.Selection.PrevBlockId = block.Id;
                            Canvas.SetLeft(selectionBox, blockX);
                            Canvas.SetTop(selectionBox, block.Y);
                        }
                        break;
                    }
                case 1:
                    {
                        if (radius < 4 && block.Id != 6 && isLeftClick)
                        {
                            Player.AddToInventory(new ItemBuildingBlock(block.Id), GuiIngame.Hotbar.Selection, 99);
                            blockChunk.Children.Remove(block);
                            Block replace = new(6);
                            replace.SetPos(block.X, block.Y);
                            replace.MouseEnter += (sender, EventArgs) => BlockTouched(replace, 0, EventArgs);
                            replace.MouseDown += (sender, EventArgs) => BlockTouched(replace, 1, EventArgs);
                            blockChunk.Children.Add(replace);
                        }
                        if (radius < 4 && block.Id == 6 && !isLeftClick)
                        {
                            if (Player.Inventory.HotbarEntries[GuiIngame.Hotbar.Selection] != null)
                            {
                                blockChunk.Children.Remove(block);
                                ItemBuildingBlock newBlock = (ItemBuildingBlock)Player.Inventory.HotbarEntries[GuiIngame.Hotbar.Selection].Item;
                                Block replace = null;
                                switch(newBlock.Block.Id)
                                {
                                    case < 10000:
                                        {
                                            replace = new(newBlock.Block.Id);
                                            break;
                                        }
                                    case 10000:
                                        {
                                            replace = new BlockChest(newBlock.Block.Id);
                                            for (int i = 0; i < 27; i++)
                                            {
                                                ItemBuildingBlock item = new ItemBuildingBlock(Rand.Next(0, 7));
                                                BlockChest c = (BlockChest)replace;
                                                c.Chest.AddItem(item, i, i);
                                            }
                                            break;
                                        }
                                }
                                replace.SetPos(block.X, block.Y);
                                replace.MouseEnter += (sender, EventArgs) => BlockTouched(replace, 0, EventArgs);
                                replace.MouseDown += (sender, EventArgs) => BlockTouched(replace, 1, EventArgs);
                                blockChunk.Children.Add(replace);
                            }
                        }
                        //if (radius < 4 && block.Id == 6 && !isLeftClick)
                        //{
                        //    blockChunk.Children.Remove(block);
                        //    BlockChest replace = new(10000);
                        //    for (int i = 0; i < 27; i++)
                        //    {
                        //        ItemBuildingBlock item = new ItemBuildingBlock(Rand.Next(0, 7));
                        //        replace.Chest.AddItem(item, i, i);
                        //    }
                        //    replace.SetPos(block.X, block.Y);
                        //    replace.MouseEnter += (sender, EventArgs) => BlockTouched(replace, 0, EventArgs);
                        //    replace.MouseDown += (sender, EventArgs) => BlockTouched(replace, 1, EventArgs);
                        //    blockChunk.Children.Add(replace);
                        //}
                        if (radius < 4 && block.Id == 10000 && !isLeftClick)
                        {
                            BlockChest chest = (BlockChest)block;
                            for (int i = 0; i < chest.Chest.Entries.Length - 1; i++)
                            {
                                ContainerEntry entry = chest.Chest.Entries[i];
                                ItemBuildingBlock item = (ItemBuildingBlock)entry.Item;
                                Debug.WriteLine($"{item.Type} - {item.Block.Id} - {entry.Count}");
                            }
                        }
                        GuiIngame.Hotbar.UpdateItems(Player);
                        break;
                    }
            }
            selectionInfo.Content = $"Selected:\n{block.X} / {block.Y}\nID: {block.Id}\nSelectionBox Pos: {Canvas.GetLeft(selectionBox)} / {Canvas.GetTop(selectionBox)}";
        }

        void UnloadOldestChunk()
        {
            List<long> list = new List<long>();
            List<Canvas> listc = new List<Canvas>();
            foreach (Canvas chunk in chunks.Children)
            {
                list.Add(Convert.ToInt64(chunk.Uid));
                long i = list.Min();
                listc.Add(chunk);
                foreach(Canvas c in listc)
                {
                    if (Convert.ToInt64(c.Uid) == i)
                    {
                        double chunkX = Canvas.GetLeft(chunk);
                        if (chunkX < Player.X - 64 || chunkX > Player.X + 64)
                        {
                            chunks.Children.Remove(c);
                            break;
                        }
                    }
                }
                break;
            }
        }

        void AskNewChunk()
        {
            int nearestPossibleX = (int)Math.Round((Player.X / 16), MidpointRounding.AwayFromZero) * 16;
            chunkGenInfo.Content = $"{nearestPossibleX}";
            bool chunkFoundAtGoaledX = false;
            foreach(Chunk chunk in chunks.Children)
            {
                double x = Canvas.GetLeft(chunk);
                if (x == nearestPossibleX)
                {
                    chunkFoundAtGoaledX = true;
                    break;
                }
            }
            chunkGenInfo.Content = $"{nearestPossibleX}\n{chunkFoundAtGoaledX}";
            if (!chunkFoundAtGoaledX)
            {
                Chunk chunk = ChunkProvider.ProvideChunk(nearestPossibleX);
                foreach(Block block in chunk.Children)
                {
                    block.MouseEnter += (sender, EventArgs) => BlockTouched(block, 0, EventArgs);
                    block.MouseDown += (sender, EventArgs) => BlockTouched(block, 1, EventArgs);
                }
                chunks.Children.Add(chunk);
            }
        }

        public void Stop(Exception e, string error, string source)
        {
            this.world.Children.Clear();
            foreach (UIElement element in windowGrid.Children)
            {
                if (element != uiStop)
                {
                    element.Visibility = Visibility.Hidden;
                }
                else
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            Canvas stopInfo = new Canvas();
            stopInfo.Width = 700;
            stopInfo.Height = 300;
            stopInfo.Background = Brushes.DarkRed;
            Label info = new Label();
            info.Foreground = Brushes.White;
            info.Width = 630;
            info.Height = 250;
            Canvas.SetLeft(info, 30);
            Canvas.SetTop(info, 30);
            string exceptionInfo = "not thrown";
            if (e != null)
            {
                exceptionInfo = e.Message;
            }
            info.Content = $"WPFCraft ran into an error.\nThe error is unrecoverable and the game has stopped.\n\n\nDetailed information about what happened:\n\n{error}\n\nWhat failed: {source}\n\nException info, if thrown: {exceptionInfo}";
            stopInfo.Children.Add(info);
            Canvas.SetLeft(stopInfo, Canvas.GetLeft(stopTitle));
            Canvas.SetTop(stopInfo, Canvas.GetTop(stopTitle) + stopTitle.Height + 5);
            uiStop.Children.Add(stopInfo);
        }

        void JoinServer(string ip, int port)
        {
            this.uiMultiplayer.Visibility = Visibility.Hidden;
            this.Server = new Server(this, Player, false, ip, port);
            this.Server.ConnectToServer(ip, port);
        }

        void OpenServer(string ip, int port)
        {
            this.InternalServer = new Server(null, null, true, ip, port);
        }

        public void PlayerMPPosUpdated(ulong id, double x, double y)
        {
            foreach (Player player in this.entities.Children)
            {
                if (player.Id == id)
                {
                    player.SetPos(x, y);
                    break;
                }
            }
        }

        void Print(string text)
        {
            Console.WriteLine(text);
            Debug.WriteLine(text);
        }
    }
}