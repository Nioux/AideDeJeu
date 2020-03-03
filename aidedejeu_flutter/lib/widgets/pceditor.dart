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

  void _initRaces() async {
    var races = await loadRaces();
    setState(() {
      _races = races.map((e) => e as RaceItem).toList();
    });
  }

  void _initSubRaces(RaceItem race) async {
    var subRaces = await loadSubRaces(race);
    setState(() {
      _subRaces = subRaces.map((e) => e as SubRaceItem).toList();
    });
  }

  void _initBackgrounds() async {
    var backgrounds = await loadBackgrounds();
    setState(() {
      _backgrounds = backgrounds.map((e) => e as BackgroundItem).toList();
    });
  }

  void _initSubBackgrounds(BackgroundItem background) async {
    var subBackgrounds = await loadSubBackgrounds(background);
    setState(() {
      _subBackgrounds =
          subBackgrounds.map((e) => e as SubBackgroundItem).toList();
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

  Widget _loadMarkdown(String markdown) {
    return MarkdownBody(
      data: markdown ?? "",
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
      ),
    );
  }

  Widget _loadItemsWidget<T extends Item>(
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

  Widget _loadRacesWidget() {
    return _loadItemsWidget<RaceItem>(
      hintText: "Race",
      items: _races,
      selectedItem: _race,
      onChanged: (value) {
        _setRace(value);
      },
    );
  }

  Widget _loadSubRacesWidget() {
    return _subRaces != null
        ? _loadItemsWidget<SubRaceItem>(
            hintText: "Variante",
            items: _subRaces,
            selectedItem: _subRace,
            onChanged: (value) {
              _setSubRace(value);
            },
          )
        : SizedBox.shrink();
  }

  Widget _loadRaceDetailsWidget() {
    return _race != null
        ? Column(
      crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text("Augmentation de caractéristiques", style: TextStyle(fontSize: 16, fontFamily: "Cinzel",)),
              _loadMarkdown(_race?.abilityScoreIncrease),
              _loadMarkdown(_subRace?.abilityScoreIncrease),
              Text(""),
              Text("Âge"),
              _loadMarkdown(_race?.age),
              Text(""),
              Text("Alignement"),
              _loadMarkdown(_race?.alignment),
              Text(""),
              Text("Taille"),
              _loadMarkdown(_race?.size),
              Text(""),
              Text("Vitesse"),
              _loadMarkdown(_race?.speed),
              Text(""),
              Text("Vision dans le noir"),
              _loadMarkdown(_race?.darkvision),
              Text(""),
              Text("Langues"),
              _loadMarkdown(_race?.languages),
            ],
          )
        : SizedBox.shrink();
  }

  Widget _loadBackgroundsWidget() {
    return _loadItemsWidget<BackgroundItem>(
      hintText: "Historique",
      items: _backgrounds,
      selectedItem: _background,
      onChanged: (value) {
        _setBackground(value);
      },
    );
  }

  Widget _loadSubBackgroundsWidget() {
    return _subBackgrounds != null
        ? _loadItemsWidget<SubBackgroundItem>(
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
            indicatorColor: Theme.of(context).accentColor, // Colors.red,
            indicatorSize: TabBarIndicatorSize.label,
            tabs: <Widget>[
              Text(
                AppLocalizations.of(context).raceTitle,
                style: TextStyle(fontSize: 25, fontFamily: "Cinzel",),
              ),
              Text(
                AppLocalizations.of(context).backgroundTitle,
                style: TextStyle(fontSize: 25, fontFamily: "Cinzel",),
              ),
              Text(
                AppLocalizations.of(context).classTitle,
                style: TextStyle(fontSize: 25, fontFamily: "Cinzel",),
              ),
              Text(
                AppLocalizations.of(context).abilitiesTitle,
                style: TextStyle(fontSize: 25, fontFamily: "Cinzel",),
              ),
              Text(
                AppLocalizations.of(context).othersTitle,
                style: TextStyle(fontSize: 25, fontFamily: "Cinzel",),
              ),
            ],
          ),
        ),
        body: TabBarView(
          children: [
            ListView(
              children: <Widget>[
                _loadRacesWidget(),
                _loadSubRacesWidget(),
                _loadRaceDetailsWidget(),
              ],
            ),
            ListView(
              children: <Widget>[
                _loadBackgroundsWidget(),
                _loadSubBackgroundsWidget(),
              ],
            ),
            Text(AppLocalizations.of(context).classTitle),
            Text(AppLocalizations.of(context).abilitiesTitle),
            Text(AppLocalizations.of(context).othersTitle),
          ],
        ),
      ),
    );
  }
}
