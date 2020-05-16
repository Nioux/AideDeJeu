import 'package:aidedejeu_flutter/localization.dart';
import 'package:aidedejeu_flutter/widgets/library.dart';
import 'package:aidedejeu_flutter/widgets/pceditor.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';

class HomePage extends StatelessWidget {
  HomePage({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text(
            AppLocalizations.of(context).appTitle,
          ),
        ),
        body: Column(
          children: <Widget>[
            FlatButton.icon(
              label: Text(
                AppLocalizations.of(context).libraryTitle,
                  style: Theme.of(context).textTheme.headline5,
              ),
              icon: SvgPicture.asset(
                "assets/spell-book.svg",
                height: 100.0,
                width: 100.0,
                allowDrawingOutsideViewBox: true,
              ),
              onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => LibraryPage(
                    id: 'index.md',
                  ),
                ),
              ),
            ),
            FlatButton.icon(
              label: Text(
                AppLocalizations.of(context).pceditorTitle,
                style: Theme.of(context).textTheme.headline5,
              ),
              icon: SvgPicture.asset(
                "assets/swordman.svg",
                height: 100.0,
                width: 100.0,
                allowDrawingOutsideViewBox: true,
              ),
              onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => PCEditorPage(),
                ),
              ),
            ),
            FlatButton.icon(
              label: Text(
                AppLocalizations.of(context).aboutTitle,
                  style: Theme.of(context).textTheme.headline5,
              ),
              icon: SvgPicture.asset(
                "assets/wooden-sign.svg",
                height: 100.0,
                width: 100.0,
                allowDrawingOutsideViewBox: true,
              ),
              onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => LibraryPage(
                    id: 'index.md',
                  ),
                ),
              ),
            ),
          ],
        ));
  }
}
