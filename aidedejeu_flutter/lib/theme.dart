import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

const Color colorHDRed = Color(0xFF9B1C47);
const Color colorHDBlue = Color(0xFF5B61FF);
const Color colorHDGrey = Color(0xFF563F5A);
const Color colorHDMidGrey = Color(0xFF6F5B73);
const Color colorHDLightGrey = Color(0xFF7C7B7B);
const Color colorHDLightBlack = Color(0xFF3A213C);
const Color colorHDBackMidGrey = Color(0xFFB5AAB9);
const Color colorHDBackLightGrey = Color(0xFFEDEDED);
const Color colorHDWhite = Color(0xFFFFFFFF);
const Color colorHDBlack = Color(0xFF000000);

ThemeData mainTheme() {
  return ThemeData(
    primarySwatch: Colors.deepOrange,
    appBarTheme: AppBarTheme(
      color: Colors.white,
      iconTheme: IconThemeData(color: Colors.black),
      textTheme: TextTheme(
          headline6: TextStyle(
              fontSize: 28.0,
              color: Colors.black,
              fontWeight: FontWeight.bold,
              fontFamily: 'Cinzel')),
    ),
    brightness: Brightness.light,
    primaryColor: colorHDBlack,
    accentColor: colorHDRed,
    fontFamily: 'LinuxLibertine',
    textTheme: TextTheme(
      // p
      bodyText2: TextStyle(
        fontSize: 15.0,
        color: Colors.black,
        fontFamily: 'LinuxLibertine',
      ),
      // h1
      headline5: TextStyle(
        fontSize: 30.0,
        color: colorHDRed,
        fontWeight: FontWeight.bold,
        fontFamily: 'Cinzel-Bold',
      ),
      // h2
      headline6: TextStyle(
        fontSize: 25.0,
        color: Colors.black,
        fontWeight: FontWeight.bold,
        fontFamily: 'Cinzel-Regular',
      ),
      // h3
      subtitle1: TextStyle(
        fontSize: 20.0,
        color: Colors.black,
        fontWeight: FontWeight.bold,
        fontFamily: 'Cinzel-Regular',
      ),
      // h4, h5, h6
      bodyText1: TextStyle(
        fontSize: 18.0,
        color: Colors.black,
        fontWeight: FontWeight.bold,
        fontFamily: 'Cinzel-Regular',
      ),
    ),
  );
}

MarkdownStyleSheet mainMarkdownStyleSheet(BuildContext context) {
  return MarkdownStyleSheet.fromTheme(Theme.of(context)).copyWith(
    a: const TextStyle(color: colorHDBlue),
    tableColumnWidth: IntrinsicColumnWidth(),
    tableCellsPadding: EdgeInsets.all(1.0),
    tableHeadAlign: TextAlign.center,
  );
}
