import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

ThemeData mainTheme() {
  return ThemeData(
    primarySwatch: Colors.deepOrange,
    appBarTheme: AppBarTheme(
      color: Colors.white,
      iconTheme: IconThemeData(color: Colors.black),
      textTheme: TextTheme(
          title: TextStyle(
              fontSize: 28.0,
              color: Colors.black,
              fontWeight: FontWeight.bold,
              fontFamily: 'Cinzel')),
    ),
    brightness: Brightness.light,
    primaryColor: Colors.lightBlue[800],
    accentColor: Colors.cyan[600],
    fontFamily: 'LinuxLibertine',
    textTheme: TextTheme(
      headline: TextStyle(
          fontSize: 28.0, fontWeight: FontWeight.bold, fontFamily: 'Cinzel'),
      title: TextStyle(fontSize: 22.0, fontStyle: FontStyle.normal),
      body1: TextStyle(fontSize: 16.0),
    ),
  );
}

MarkdownStyleSheet mainMarkdownStyleSheet(BuildContext context) {
  return MarkdownStyleSheet.fromTheme(Theme.of(context)).copyWith(
    tableColumnWidth: IntrinsicColumnWidth(),
    tableCellsPadding: EdgeInsets.all(0.2),
  );
}
