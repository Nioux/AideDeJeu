import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:flutter_svg/flutter_svg.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.red,
      ),
      home: MyHomePage(id: 'index.md'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, @required this.id}) : super(key: key);

  final String id;

  @override
  _MyHomePageState createState() => _MyHomePageState(id: this.id);
}

class _MyHomePageState extends State<MyHomePage> {
  _MyHomePageState({@required this.id});

  final String id;

  void setItem(Item item) {
    setState(() {
      this.item = item;
      this.markdown =
          item.Markdown.replaceAllMapped(RegExp(r'<!--.*?-->'), (match) {
        return '';
      });
    });
  }

  String markdown = "";
  Item item = null;
  MarkdownStyleSheet styleSheet;

  @override
  void initState() {
    super.initState();

    ThemeData theme = ThemeData(
      brightness: Brightness.light,
      primaryColor: Colors.lightBlue[800],
      accentColor: Colors.cyan[600],
      fontFamily: 'LinuxLibertine',
      textTheme: TextTheme(
        headline: TextStyle(fontSize: 28.0, fontWeight: FontWeight.bold),
        title: TextStyle(fontSize: 22.0, fontStyle: FontStyle.italic),
        body1: TextStyle(fontSize: 16.0, fontFamily: 'Hind'),
      ),
    );

    styleSheet = MarkdownStyleSheet.fromTheme(theme).copyWith(
        tableColumnWidth: IntrinsicColumnWidth(),
        tableCellsPadding: EdgeInsets.all(0.2));

    loadItem().then((item) => setItem(item));
  }

  Future<Item> loadItem() async {
    var item = await getItemWithId(this.id);
    var items = await loadChildrenItems(item);
    //setItem(item);
    return item;
  }

  Widget buildMarkdown(BuildContext context) {
    return Markdown(
      data: markdown,
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: link)),
      ),
    );
  }

  Widget buildMarkdownBody(BuildContext context) {
    return MarkdownBody(
      data: markdown,
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: link)),
      ),
    );
  }

  Widget buildChildTile(BuildContext context, Item item) {
    return ListTile(
      title: Text(item.Name),
      subtitle: Text(item.AliasText ?? ""),
      onTap: () => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: item.Id)),
      ),
    );
  }

  Widget buildLibraryPage() {
    return Stack(
      children: <Widget>[
        ListView.builder(
            itemCount: (item?.Children?.length ?? 0) + 1,
            itemBuilder: (BuildContext context, int index) {
              return index == 0
                  ? buildMarkdownBody(context)
                  : buildChildTile(context, item.Children[index - 1]);
            })
      ],
    );
  }

  Widget buildBookmarksPage() {
    return Text("Bookmarks");
  }

  Widget buildSearchPage() {
    return Text("Search");
  }

  BottomNavigationBarItem buildBottomNavigationBarItem(
      String title, String assetName) {
    return BottomNavigationBarItem(
      icon: SvgPicture.asset(
        assetName,
        height: 30.0,
        width: 30.0,
        allowDrawingOutsideViewBox: true,
      ),
      title: Text(title),
      activeIcon: SvgPicture.asset(
        assetName,
        height: 40.0,
        width: 40.0,
        allowDrawingOutsideViewBox: true,
      ),
    );
  }

  List buildBottomNavigationBarItems() {
    return <BottomNavigationBarItem>[
      buildBottomNavigationBarItem("Bibliothèque", "assets/spell-book.svg"),
      buildBottomNavigationBarItem("Favoris", "assets/stars-stack.svg"),
      buildBottomNavigationBarItem("Recherche", "assets/crystal-ball.svg"),
    ];
  }

  int indexPage = 0;

  @override
  Widget build(BuildContext context) {
    Widget currentPage;
    switch (indexPage) {
      case 0:
        currentPage = buildLibraryPage();
        break;
      case 1:
        currentPage = buildBookmarksPage();
        break;
      case 2:
        currentPage = buildSearchPage();
        break;
    }

    return Scaffold(
      appBar: AppBar(
        title: Text(widget.id),
      ),
      body: currentPage,
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: indexPage,
        onTap: (int index) {
          setState(() {
            this.indexPage = index;
          });
        },
        items: buildBottomNavigationBarItems(),
      ),
      endDrawer: Drawer(
        child: ListView(
          // Important: Remove any padding from the ListView.
          padding: EdgeInsets.zero,
          children: <Widget>[
            DrawerHeader(
              child: Text('Drawer Header'),
              decoration: BoxDecoration(
                color: Colors.blue,
              ),
            ),
            ListTile(
              title: Text('Item 1'),
              onTap: () {
                // Update the state of the app.
                // ...
              },
            ),
            ListTile(
              title: Text('Item 2'),
              onTap: () {
                // Update the state of the app.
                // ...
              },
            ),
            Container(
              child: Wrap(
                children: <Widget>[
                  FilterChip(
                    label: Text("truc"),
                    backgroundColor: Colors.transparent,
                    shape: StadiumBorder(side: BorderSide()),
                    onSelected: (bool value) {
                      print("selected");
                    },
                  ),
                  FilterChip(
                    label: Text("truc"),
                    backgroundColor: Colors.transparent,
                    shape: StadiumBorder(side: BorderSide()),
                    onSelected: (bool value) {
                      print("selected");
                    },
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
