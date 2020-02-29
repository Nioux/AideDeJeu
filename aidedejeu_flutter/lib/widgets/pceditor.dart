import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/filters.dart';
import 'package:aidedejeu_flutter/widgets/filters.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:aidedejeu_flutter/widgets/library.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:flutter_svg/flutter_svg.dart';

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
  RaceItem race;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Personnage"),
      ),
      body: Column(
        children: <Widget>[
          race != null
              ? Text(race.name)
              : DropdownButton(hint: Text("Race"), value: "")
        ],
      ),
    );
  }
}
