import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/widgets/filters.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:flutter_svg/flutter_svg.dart';

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, @required this.id}) : super(key: key);

  final String id;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  void setItem(Item item) {
    setState(() {
      this.item = item;
      if (item is FilteredItems) {
        this.filters = (item as FilteredItems).toFilterList();
      } else {
        this.filters = null;
      }
      this.markdown =
          item.Markdown.replaceAllMapped(RegExp(r'<!--.*?-->'), (match) {
        return '';
      });
    });
  }

  String markdown = "";
  Item item = null;
  MarkdownStyleSheet styleSheet;
  List<Filter> filters = null;

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

    _loadItem().then((item) => setItem(item));
  }

  Future<Item> _loadItem() async {
    var item = await getItemWithId(this.widget.id);
    var items = await loadChildrenItems(item, filters);
    //setItem(item);
    return item;
  }

  Widget _buildMarkdown(BuildContext context) {
    return Markdown(
      data: markdown,
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: link)),
      ),
    );
  }

  Widget _buildMarkdownBody(BuildContext context) {
    return MarkdownBody(
      data: markdown,
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: link)),
      ),
    );
  }

  Widget _buildChildTile(BuildContext context, Item item) {
    return ListTile(
      title: Text(item.Name),
      subtitle: Text(item.AliasText ?? ""),
      onTap: () => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => MyHomePage(id: item.Id)),
      ),
    );
  }

  Widget _buildLibraryPage() {
    return Stack(
      children: <Widget>[
        ListView.builder(
            itemCount: (item?.Children?.length ?? 0) + 1,
            itemBuilder: (BuildContext context, int index) {
              return index == 0
                  ? _buildMarkdownBody(context)
                  : _buildChildTile(context, item.Children[index - 1]);
            })
      ],
    );
  }

  Widget _buildBookmarksPage() {
    return Text("Bookmarks");
  }

  Widget _buildSearchPage() {
    return Text("Search");
  }

  BottomNavigationBarItem _buildBottomNavigationBarItem(
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

  List _buildBottomNavigationBarItems() {
    return <BottomNavigationBarItem>[
      _buildBottomNavigationBarItem("Bibliothèque", "assets/spell-book.svg"),
      _buildBottomNavigationBarItem("Favoris", "assets/stars-stack.svg"),
      _buildBottomNavigationBarItem("Recherche", "assets/crystal-ball.svg"),
    ];
  }

  Widget _buildChoiceFilter(Filter filter) {
    return ChipListFilter(
      choices: filter.values,
      selectedChoices: filter.selectedValues,
      updateSelectedChoices: (Set<String> choices) {
        setState(() {
          filter.selectedValues = choices;
        });
        loadChildrenItems(item, filters).then((value) => {
              setState(() {
                this.item = item;
                this.filters = filters;
              })
            });
      },
    );
  }

  Widget _buildRangeFilter(Filter filter) {
    return RangeFilter(
        values: filter.values,
        rangeValues: filter.rangeValues,
        updateRangeValues: (RangeValues values) {
          setState(() {
            filter.rangeValues = values;
          });
        });
  }

  Widget _buildFilter(Filter filter) {
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
              ? _buildChoiceFilter(filter)
              : _buildRangeFilter(filter))
    ]);
  }

  List<Widget> _buildFilterList() {
    return filters.map((filter) => _buildFilter(filter)).toList();
  }

  int indexPage = 0;

  @override
  Widget build(BuildContext context) {
    print("build");
    Widget currentPage;
    switch (indexPage) {
      case 0:
        currentPage = _buildLibraryPage();
        break;
      case 1:
        currentPage = _buildBookmarksPage();
        break;
      case 2:
        currentPage = _buildSearchPage();
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
        items: _buildBottomNavigationBarItems(),
      ),
      endDrawer: filters != null
          ? Drawer(
              child: ListView(
                  // Important: Remove any padding from the ListView.
                  padding: EdgeInsets.zero,
                  children: _buildFilterList()),
            )
          : null,
      appBar: AppBar(
        title: Text(widget.id),
        actions: filters != null
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
