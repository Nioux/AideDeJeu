using OnePlat.DiceNotation;
using OnePlat.DiceNotation.DieRoller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;

namespace AideDeJeu.Views
{
    [Preserve(AllMembers = true)]
    public class Charts : Application
    {
        bool movementsEnabled;
        //Scene scene;
        Node plotNode;
        //Camera camera;
        Octree octree;
        List<Bar> bars;
        Viewport vp;
        Renderer renderer;

        public void DoStop()
        {
            base.Exit(); //.Stop();
        }

        public Bar SelectedBar { get; private set; }

        public IEnumerable<Bar> Bars => bars;

        [Preserve]
        public Charts(ApplicationOptions options = null) : base(options)
        {
            var i = 0;
        }

        public Charts() : base(null)
        {

        }

        static Charts()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }

        protected override void Start()
        {
            base.Start();
            vp = new Viewport(Context, null, null, null);
            vp.SetClearColor(new Color(1, 1, 1, 1));
            Renderer.SetViewport(0, vp);
            //SetupViewport();
            CreateScene();
        }

        async void CreateScene()
        {
            Input.TouchEnd += OnTouched;
            //Input.SubscribeToTouchEnd(OnTouched);

            vp.Scene = new Scene();            
            octree = vp.Scene.CreateComponent<Octree>();

            plotNode = vp.Scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();
            //var plane = baseNode.CreateComponent<StaticModel>();
            //plane.Model = CoreAssets.Models.Plane;
            //plane.Material = Material.FromColor(new Color(0, 0, 0, 1));

            var cameraNode = vp.Scene.CreateChild();
            vp.Camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(10, 15, 10) / (1f);
            cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;



            //var diceRoller = new ViewModels.DiceRollerViewModel();
            //var diceRolls = diceRoller.DicesValues(6, 3);
            //float sumx = diceRolls.Sum(kv => kv.Value);

            int sizei = 6;
            int sizej = 1;
            baseNode.Scale = new Vector3(sizei * 1.5f, 1, sizej * 1.5f);
            bars = new List<Bar>(sizei * sizej);

            //DrawRolls(diceRolls, sumx, 0, 1);
            for (int j = 0; j < sizej; j++)
            {
                for (int i = 0; i < sizei; i ++)
                {
                    IDice dice = new Dice();
                    var diceResult = dice.Roll("3d6", new RandomDieRoller());

                    var boxNode = plotNode.CreateChild();
                    boxNode.Position = new Vector3(sizei / 2f - (float)i * 1.5f, 0, sizej / 2f - (float)j * 1.5f);
                    //var bar = new Bar(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                    var bar = new Bar(new Color(1f - (((float)diceResult.Value - 3f) / 15f), (((float)diceResult.Value - 3f) / 15f), 0, 0.9f));
                    boxNode.AddComponent(bar);
                    //bar.SetValueWithAnimation((Math.Abs(i) + Math.Abs(j) + 1) / 2f);
                    bar.SetValueWithAnimation(diceResult.Value);
                    //bar.SetValueWithAnimation(diceRolls[idice + 3] / 10);
                    bars.Add(bar);
                }
            }

            SelectedBar = bars.First();
            SelectedBar.Select();



            try
            {
                await plotNode.RunActionsAsync(new EaseBackOut(new RotateBy(2f, 0, 360, 0)));
            }
            catch (OperationCanceledException) { }
            movementsEnabled = true;
        }


        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = vp.Camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry);
            if (results != null)
            {
                var bar = results.Value.Node?.Parent?.GetComponent<Bar>();
                if (SelectedBar != bar)
                {
                    SelectedBar?.Deselect();
                    SelectedBar = bar;
                    SelectedBar?.Select();
                }
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            if (Input.NumTouches >= 1 && movementsEnabled)
            {
                var touch = Input.GetTouch(0);
                plotNode.Rotate(new Quaternion(0, -touch.Delta.X, 0), TransformSpace.Local);
            }
            base.OnUpdate(timeStep);
        }

        public void Rotate(float toValue)
        {
            plotNode.Rotate(new Quaternion(0, toValue, 0), TransformSpace.Local);
        }

        void SetupViewport()
        {
            //renderer = Renderer;
            //vp = new Viewport(Context, scene, camera, null);
            //vp.SetClearColor(new Color(1, 1, 1, 1));
            //renderer.SetViewport(0, vp);
        }
    }

    public class Bar : Component
    {
        Node barNode;
        Node textNode;
        Text3D text3D;
        Color color;
        float lastUpdateValue;

        public float Value
        {
            get { return barNode.Scale.Y; }
            set { barNode.Scale = new Vector3(1, value < 0.3f ? 0.3f : value, 1); }
        }

        private float finalValue { get; set; }

        public void SetValueWithAnimation(float value)
        {
            finalValue = value;
            barNode.RunActionsAsync(new EaseBackOut(new ScaleTo(3f, 1, value / 2, 1)));
            barNode.RunActionsAsync(new EaseBackOut(new TintTo(3f, new Color(1f - (((float)value - 3f) / 15f), (((float)value - 3f) / 15f), 0, 0.9f))));
        }

        public Bar(Color color)
        {
            this.color = color;
            ReceiveSceneUpdates = true;
        }

        public override void OnAttachedToNode(Node node)
        {
            barNode = node.CreateChild();
            barNode.Scale = new Vector3(1, 0, 1); //means zero height
            var box = barNode.CreateComponent<Box>();
            box.Color = color;

            textNode = node.CreateChild();
            textNode.Rotate(new Quaternion(0, 180, 0), TransformSpace.World);
            textNode.Position = new Vector3(0, 10, 0);
            text3D = textNode.CreateComponent<Text3D>();
            text3D.SetFont(CoreAssets.Fonts.AnonymousPro, 60);
            //var font = new Font();
            //var fontStream = Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.Cinzel-Regular.otf");
            //var memoryStream = new MemoryStream();
            //fontStream.CopyTo(memoryStream);
            //memoryStream.Seek(0, SeekOrigin.Begin);
            //MemoryBuffer mb = new MemoryBuffer(memoryStream);
            //bool res = font.Load(mb);
            //text3D.SetFont(font, 60);
            text3D.TextEffect = TextEffect.Stroke;

            base.OnAttachedToNode(node);
        }

        protected override void OnUpdate(float timeStep)
        {
            var pos = barNode.Position;
            var scale = barNode.Scale;
            barNode.Position = new Vector3(pos.X, scale.Y / 2f, pos.Z);
            textNode.Position = new Vector3(0.5f, scale.Y + 0.2f, 0);
            //var newValue = (float)Math.Round(scale.Y, 1);
            //if (lastUpdateValue != newValue)
            //text3D.TextAlignment = HorizontalAlignment.Center;
                text3D.Text = finalValue.ToString();// newValue.ToString("F01", CultureInfo.InvariantCulture);
            //lastUpdateValue = newValue;
        }

        public void Deselect()
        {
            barNode.RemoveAllActions();//TODO: remove only "selection" action
            barNode.RunActions(new EaseBackOut(new TintTo(1f, color.R, color.G, color.B)));
        }

        public void Select()
        {
            Selected?.Invoke(this);
            // "blinking" animation
            barNode.RunActions(new RepeatForever(new TintTo(0.3f, 1f, 1f, 1f), new TintTo(0.3f, color.R, color.G, color.B)));
        }

        public event Action<Bar> Selected;
    }

    //public static class RandomHelper
    //{
    //    static readonly Random random = new Random();

    //    /// <summary>
    //    /// Return a random float between 0.0 (inclusive) and 1.0 (exclusive.)
    //    /// </summary>
    //    public static float NextRandom() { return (float)random.NextDouble(); }

    //    /// <summary>
    //    /// Return a random float between 0.0 and range, inclusive from both ends.
    //    /// </summary>
    //    public static float NextRandom(float range) { return (float)random.NextDouble() * range; }

    //    /// <summary>
    //    /// Return a random float between min and max, inclusive from both ends.
    //    /// </summary>
    //    public static float NextRandom(float min, float max) { return (float)((random.NextDouble() * (max - min)) + min); }

    //    /// <summary>
    //    /// Return a random integer between min and max - 1.
    //    /// </summary>
    //    public static int NextRandom(int min, int max) { return random.Next(min, max); }
    //}
}
