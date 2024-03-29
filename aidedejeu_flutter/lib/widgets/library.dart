import 'package:aidedejeu_flutter/databases/database.dart';
import 'package:aidedejeu_flutter/databases/database_sqflite.dart';
import 'package:aidedejeu_flutter/localization.dart';
import 'package:aidedejeu_flutter/models/filters.dart';
import 'package:aidedejeu_flutter/widgets/filters.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:flutter_svg/flutter_svg.dart';

import '../theme.dart';

class LibraryPage extends StatefulWidget {
  LibraryPage({Key key, @required this.id}) : super(key: key);

  final String id;
  final BaseDB _db = SqfliteDB.instance;

  @override
  _LibraryPageState createState() => _LibraryPageState();
}

class _LibraryPageState extends State<LibraryPage> {
  final BaseDB _db = SqfliteDB.instance;
  void setItem(Item item) {
    setState(() {
      this.item = item;
      if (item is FilteredItems) {
        this.filters = item.toFilterList();
      } else {
        this.filters = null;
      }
      this.markdown =
          item.markdown.replaceAllMapped(RegExp(r'<!--.*?-->'), (match) {
        return '';
      });
    });
  }

  String markdown = "";
  Item item;
  MarkdownStyleSheet styleSheet;
  List<Filter> filters;

  @override
  void initState() {
    super.initState();

    _loadItem().then((item) => setItem(item));
  }

  @protected
  @mustCallSuper
  void didChangeDependencies() {
    super.didChangeDependencies();
    styleSheet = mainMarkdownStyleSheet(context);
  }

  Future<Item> _loadItem() async {
    var item = await _db.getItemWithId(this.widget.id);
    await _db.loadChildrenItems(item, filters);
    return item;
  }

  Widget _buildMarkdown(BuildContext context) {
    return Markdown(
      data: markdown,
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
      ),
    );
  }

  Widget _buildMarkdownBody(BuildContext context) {
    return Container(
        margin: const EdgeInsets.all(10.0),
        child: MarkdownBody(
          data: markdown,
          styleSheet: styleSheet,
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ));
  }

  Widget _buildChildTile(BuildContext context, Item item) {
    return ListTile(
      title: Text(
        item.name,
        style: TextStyle(
          fontSize: 25.0,
          color: colorHDBlue,
          fontWeight: FontWeight.bold,
          fontFamily: 'Cinzel-Regular',
        ),
      ),
      subtitle: Text(item.altNameText ?? "",
        style: TextStyle(
          fontSize: 18.0,
          color: colorHDLightGrey,
          fontWeight: FontWeight.bold,
          fontFamily: 'Cinzel-Regular',
        ),),
      onTap: () => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => LibraryPage(id: item.id)),
      ),
    );
  }

  Widget _buildLibraryItemPage() {
    return Stack(
      children: <Widget>[
        ListView.builder(
            itemCount: (item?.children?.length ?? 0) + 1,
            itemBuilder: (BuildContext context, int index) {
              return index == 0
                  ? _buildMarkdownBody(context)
                  : _buildChildTile(context, item.children[index - 1]);
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
      title: Text(title, style: TextStyle(fontSize: 16.0,
        fontWeight: FontWeight.bold,
        fontFamily: 'Cinzel-Regular',),),
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
      _buildBottomNavigationBarItem(
        AppLocalizations.of(context).libraryTitle,
        "assets/spell-book.svg",
      ),
      _buildBottomNavigationBarItem(
        AppLocalizations.of(context).bookmarksTitle,
        "assets/stars-stack.svg",
      ),
      _buildBottomNavigationBarItem(
        AppLocalizations.of(context).searchTitle,
        "assets/crystal-ball.svg",
      ),
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
        _db.loadChildrenItems(item, filters).then((value) => {
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
          _db.loadChildrenItems(item, filters).then((value) => {
                setState(() {
                  this.item = item;
                  this.filters = filters;
                })
              });
        });
  }

  Widget _buildFilter(Filter filter) {
    return Column(children: <Widget>[
      Divider(
        color: Colors.blueGrey,
        height: 0.0,
      ),
      Align(
        alignment: Alignment.centerLeft,
        child: Padding(
          padding: const EdgeInsets.fromLTRB(8.0,1.0,8.0,1.0),
          child: Text(AppLocalizations.of(context).translate(filter.name), style: TextStyle(fontFamily: "Cinzel"),),
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
        currentPage = _buildLibraryItemPage();
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
        selectedItemColor: colorHDRed,
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
        title: Text(item?.name ?? widget.id),
        actions: filters != null
            ? [
                Builder(
                  builder: (context) => IconButton(
                    icon: SvgPicture.asset(
                      "assets/funnel.svg",
                      height: 30.0,
                      width: 30.0,
                      allowDrawingOutsideViewBox: true,
                    ),
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
