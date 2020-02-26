import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/filterWidgets.dart';
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
      title: 'Haches & Dés',
      theme: ThemeData(
        primarySwatch: Colors.deepOrange,
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

  List<Widget> buildFilterChipList(List<String> choices) {
    return choices
        .map((choice) => Padding(
            padding: const EdgeInsets.all(4.0),
            child: FilterChip(
              label: Text(choice),
              backgroundColor: Colors.transparent,
              shape: StadiumBorder(side: BorderSide()),
              onSelected: (bool value) {
                print("selected");
              },
            )))
        .toList();
  }

  Widget buildChoiceFilter(Filter filter) {
    return Wrap(children: buildFilterChipList(filter.values));
  }

  Widget buildRangeFilter(Filter filter) {
    /*return RangeSlider(
        min: 0,
        max: 1.0 * filter.values.length,
        divisions: filter.values.length,
        labels: RangeLabels(
            'début ${filter.values.first}', 'fin ${filter.values.last}'),
        values: RangeValues(0, 1.0 * filter.values.length),
        onChanged: (RangeValues value) {});*/
    return RangeFilter(filter: filter);
  }

  Widget buildFilter(Filter filter) {
    return Column(children: <Widget>[
      Divider(
        color: Colors.blueGrey,
        height: 10.0,
      ),
      Align(
        alignment: Alignment.centerLeft,
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Text(filter.name),
        ),
      ),
      Container(
          child: filter.type == FilterType.Choices
              ? buildChoiceFilter(filter)
              : buildRangeFilter(filter))
    ]);
  }

  List<Widget> buildFilterList() {
    return (item as FilteredItems)
        .toFilterList()
        .map((filter) => buildFilter(filter))
        .toList();
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
      //appBar: AppBar(
      //  title: Text(widget.id),
      //),
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
      endDrawer: item is FilteredItems
          ? Drawer(
              child: ListView(
                  // Important: Remove any padding from the ListView.
                  padding: EdgeInsets.zero,
                  children: buildFilterList()),
            )
          : null,
      appBar: AppBar(
        title: Text(widget.id),
        actions: item is FilteredItems
            ? [
                Builder(
                  builder: (context) => IconButton(
                    icon: SvgPicture.asset(
                      "assets/funnel.svg",
                      height: 30.0,
                      width: 30.0,
                      allowDrawingOutsideViewBox: true,
                    ), //Icon(Icons.filter),
                    onPressed: () => Scaffold.of(context).openEndDrawer(),
                    tooltip:
                        MaterialLocalizations.of(context).openAppDrawerTooltip,
                  ),
                ),
              ]
            : null,
      ),
    );
  }
}
