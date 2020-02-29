import 'package:aidedejeu_flutter/widgets/homepage.dart';
import 'package:flutter/material.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Haches & Dés',
      theme: ThemeData(
        primarySwatch: Colors.deepOrange,
        appBarTheme: AppBarTheme(
          color: Colors.white,
          iconTheme: IconThemeData(color: Colors.black),
          textTheme: TextTheme(
              headline6: TextStyle(fontSize: 28.0, color: Colors.black, fontWeight: FontWeight.bold, fontFamily: 'Cinzel')),
        ),
        brightness: Brightness.light,
        primaryColor: Colors.lightBlue[800],
        accentColor: Colors.cyan[600],
        fontFamily: 'LinuxLibertine',
        textTheme: TextTheme(
          headline5: TextStyle(fontSize: 28.0, fontWeight: FontWeight.bold, fontFamily: 'Cinzel'),
          headline6: TextStyle(fontSize: 22.0, fontStyle: FontStyle.normal),
          bodyText2: TextStyle(fontSize: 16.0),
        ),
      ),
      home: HomePage(),
    );
  }
}
