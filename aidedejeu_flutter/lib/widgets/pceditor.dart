import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:aidedejeu_flutter/widgets/library.dart';
import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

class PCEditorPage extends StatefulWidget {
  PCEditorPage({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text("Personnage"),
        ),
        body: Column());
  }

  @override
  State<StatefulWidget> createState() => _PCEditorPageState();
}

class _PCEditorPageState extends State<PCEditorPage> {
  RaceItem _race;
  SubRaceItem _subRace;
  List<RaceItem> _races;
  List<SubRaceItem> _subRaces;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initRaces();
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

  Widget _loadRacesWidget() {
    return DropdownButton(
      hint: Text("Race"),
      value: _races != null ? _race : "",
      onChanged: (value) {
        _setRace(value);
      },
      items: _races != null
          ? _races
              .map((e) => DropdownMenuItem(
                    value: e,
                    child: Text(e.name),
                  ))
              .toList()
          : null,
    );
  }

  Widget _loadRaceSubRaceWidget() {
    return _race != null ? Column(
      children: [
        Text("Augmentation de caractéristiques"),
        MarkdownBody(
          data: (_race?.abilityScoreIncrease ?? "") +
              "\n\n" +
              (_subRace?.abilityScoreIncrease ?? ""),
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Âge"),
        MarkdownBody(
          data: _race?.age ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Alignement"),
        MarkdownBody(
          data: _race?.alignment ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Taille"),
        MarkdownBody(
          data: _race?.size ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Vitesse"),
        MarkdownBody(
          data: _race?.speed ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Vision dans le noir"),
        MarkdownBody(
          data: _race?.darkvision ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
        Text("Langues"),
        MarkdownBody(
          data: _race?.languages ?? "",
          onTapLink: (link) => Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
          ),
        ),
      ],
    ): SizedBox.shrink();
  }

  Widget _loadSubRacesWidget() {
    return _subRaces != null
        ? DropdownButton(
            hint: Text("Sous-Race"),
            value: _subRaces != null ? _subRace : "",
            onChanged: (value) {
              _setSubRace(value);
            },
            items: _subRaces != null
                ? _subRaces
                    .map((e) => DropdownMenuItem(
                          value: e,
                          child: Text(e.name),
                        ))
                    .toList()
                : null,
          )
        : SizedBox.shrink();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Personnage"),
      ),
      body: ListView(
        children: <Widget>[
          _loadRacesWidget(),
          _loadSubRacesWidget(),
          _loadRaceSubRaceWidget()
        ],
      ),

    );
  }
}
