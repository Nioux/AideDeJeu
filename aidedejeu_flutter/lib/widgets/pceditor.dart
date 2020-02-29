import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/material.dart';

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
    loadRaces().then((value) => setState(() {
          _races = value.map((e) => e as RaceItem).toList();
        }));
  }

  void _setRace(RaceItem race) {
    setState(() {
      this._race = race;
      this._subRace = null;
      this._subRaces = null;
    });
    loadSubRaces(race).then((value) => setState(() {
          _subRaces = value.map((e) => e as SubRaceItem).toList();
        }));
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
      body: Column(
        children: <Widget>[_loadRacesWidget(), _loadSubRacesWidget()],
      ),
    );
  }
}
