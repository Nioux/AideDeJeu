import 'package:aidedejeu_flutter/localization.dart';
import 'package:aidedejeu_flutter/theme.dart';
import 'package:aidedejeu_flutter/widgets/homepage.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      //title: 'Haches & Dés',
      onGenerateTitle: (BuildContext context) => AppLocalizations.of(context).title,
      localizationsDelegates: [
        const AppLocalizationsDelegate(),
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ],
      supportedLocales: [
        const Locale('en', ''),
        const Locale('fr', ''),
      ],
      theme: mainTheme(),
      home: HomePage(),
    );
  }
}

