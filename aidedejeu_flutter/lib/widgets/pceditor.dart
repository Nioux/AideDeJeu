import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/localization.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:aidedejeu_flutter/theme.dart';
import 'package:aidedejeu_flutter/widgets/library.dart';
import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

class PCEditorPage extends StatefulWidget {
  PCEditorPage({Key key}) : super(key: key);

  @override
  State<StatefulWidget> createState() => _PCEditorPageState();
}

class _PCEditorPageState extends State<PCEditorPage> {
  MarkdownStyleSheet styleSheet;

  RaceItem _race;
  SubRaceItem _subRace;
  List<RaceItem> _races;
  List<SubRaceItem> _subRaces;

  BackgroundItem _background;
  SubBackgroundItem _subBackground;
  List<BackgroundItem> _backgrounds;
  List<SubBackgroundItem> _subBackgrounds;

  // inits

  @override
  void initState() {
    super.initState();
    _initRaces();
    _initBackgrounds();
  }

  @protected
  @mustCallSuper
  void didChangeDependencies() {
    super.didChangeDependencies();
    styleSheet = mainMarkdownStyleSheet(context);
  }

  void _initRaces() async {
    var races = await loadRaces(context);
    setState(() {
      _races = races.toList();
    });
  }

  void _initSubRaces(RaceItem race) async {
    var subRaces = await loadSubRaces(context, race);
    setState(() {
      _subRaces = subRaces;
    });
  }

  void _initBackgrounds() async {
    var backgrounds = await loadBackgrounds(context);
    setState(() {
      _backgrounds = backgrounds;
    });
  }

  void _initSubBackgrounds(BackgroundItem background) async {
    var subBackgrounds = await loadSubBackgrounds(context, background);
    setState(() {
      _subBackgrounds =
          subBackgrounds;
    });
  }

  // setters

  void _setRace(RaceItem race) {
    setState(() {
      this._race = race;
      this._subRace = null;
      this._subRaces = null;
    });
    _initSubRaces(race);
  }

  void _setSubRace(SubRaceItem subRace) {
    setState(() {
      this._subRace = subRace;
    });
  }

  void _setBackground(BackgroundItem background) {
    setState(() {
      this._background = background;
      this._subBackground = null;
      this._subBackgrounds = null;
    });
    _initSubBackgrounds(background);
  }

  void _setSubBackground(SubBackgroundItem subBackground) {
    setState(() {
      this._subBackground = subBackground;
    });
  }

  // widgets generics

  Widget _buildMarkdown(String markdown) {
    return MarkdownBody(
      data: markdown ?? "",
      styleSheet: styleSheet,
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
      ),
    );
  }

  Widget _buildSubTitle(String title) {
    return Text(title,
        style: TextStyle(
          fontSize: 16,
          fontFamily: "Cinzel",
        ));
  }

  Widget _buildItemsWidget<T extends Item>(
      {String hintText,
      List<T> items,
      T selectedItem,
      ValueChanged<T> onChanged}) {
    return DropdownButton(
      hint: Text(hintText),
      value: items != null ? selectedItem : "",
      onChanged: (value) {
        onChanged(value);
      },
      items: items != null
          ? items
              .map((e) => DropdownMenuItem(
                    value: e,
                    child: Text(e.name),
                  ))
              .toList()
          : null,
    );
  }

  // widgets specifics

  Widget _buildRacesWidget() {
    return _buildItemsWidget<RaceItem>(
      hintText: "Race",
      items: _races,
      selectedItem: _race,
      onChanged: (value) {
        _setRace(value);
      },
    );
  }

  Widget _buildSubRacesWidget() {
    return _subRaces != null
        ? _buildItemsWidget<SubRaceItem>(
            hintText: "Variante",
            items: _subRaces,
            selectedItem: _subRace,
            onChanged: (value) {
              _setSubRace(value);
            },
          )
        : SizedBox.shrink();
  }

  Widget _buildRaceDetailsWidget() {
    return _race != null
        ? Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildSubTitle(AppLocalizations.of(context).raceAbilityScoreIncrease),
              _buildMarkdown(_race?.abilityScoreIncrease),
              _buildMarkdown(_subRace?.abilityScoreIncrease),
              Text(""),
              _buildSubTitle(AppLocalizations.of(context).raceAge),
              _buildMarkdown(_race?.age),
              Text(""),
              _buildSubTitle(AppLocalizations.of(context).raceAlignment),
              _buildMarkdown(_race?.alignment),
              Text(""),
              _buildSubTitle(AppLocalizations.of(context).raceSize),
              _buildMarkdown(_race?.size),
              Text(""),
              _buildSubTitle(AppLocalizations.of(context).raceSpeed),
              _buildMarkdown(_race?.speed),
              _race?.darkvision != null ? Text("") : SizedBox.shrink(),
              _race?.darkvision != null ? _buildSubTitle(AppLocalizations.of(context).raceDarkvision) : SizedBox.shrink(),
              _race?.darkvision != null ? _buildMarkdown(_race?.darkvision) : SizedBox.shrink(),
              Text(""),
              _buildSubTitle(AppLocalizations.of(context).raceLanguages),
              _buildMarkdown(_race?.languages),
            ],
          )
        : SizedBox.shrink();
  }

  Widget _buildBackgroundsWidget() {
    return _buildItemsWidget<BackgroundItem>(
      hintText: "Historique",
      items: _backgrounds,
      selectedItem: _background,
      onChanged: (value) {
        _setBackground(value);
      },
    );
  }

  Widget _buildSubBackgroundsWidget() {
    return _subBackgrounds != null
        ? _buildItemsWidget<SubBackgroundItem>(
            hintText: "Variante",
            items: _subBackgrounds,
            selectedItem: _subBackground,
            onChanged: (value) {
              _setSubBackground(value);
            },
          )
        : SizedBox.shrink();
  }

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 5,
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context).pceditorTitle),
          bottom: TabBar(
            labelColor: Colors.black,
            isScrollable: true,
            indicatorColor: Theme.of(context).accentColor,
            // Colors.red,
            indicatorSize: TabBarIndicatorSize.label,
            tabs: <Widget>[
              Text(
                AppLocalizations.of(context).raceTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).backgroundTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).classTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).abilitiesTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).othersTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
            ],
          ),
        ),
        body: TabBarView(
          children: [
            Container(
              margin: EdgeInsets.all(10.0),
              child: ListView(
                children: <Widget>[
                  _buildRacesWidget(),
                  _buildSubRacesWidget(),
                  _buildRaceDetailsWidget(),
                ],
              ),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: ListView(
                children: <Widget>[
                  _buildBackgroundsWidget(),
                  _buildSubBackgroundsWidget(),
                ],
              ),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).classTitle),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).abilitiesTitle),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).othersTitle),
            ),
          ],
        ),
      ),
    );
  }
}
