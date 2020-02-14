import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_markdown/flutter_markdown.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';

void main() => runApp(MyApp());

class Item {
  String Id;
  String Markdown;

  Item({this.Id, this.Markdown});

  factory Item.fromMap(Map<String, dynamic> json) => new Item(
    Id: json["Id"],
    Markdown: json["Markdown"],
  );

  Map<String, dynamic> toMap() => {
    "Id": Id,
    "Markdown": Markdown,
  };
}

Database _database;

Future<Database> get database async {
  if (_database != null) return _database;
  _database = await getDatabaseInstance();
  return _database;
}

Future<Database> getDatabaseInstance() async {
  var databasesPath = await getDatabasesPath();
  var path = join(databasesPath, "library.db");

  var exists = await databaseExists(path);
  if (!exists) {
    print("Creating new copy from asset");

    try {
      await Directory(dirname(path)).create(recursive: true);
    } catch (_) {}

    ByteData data = await rootBundle.load(join("assets", "library.db"));
    List<int> bytes =
    data.buffer.asUint8List(data.offsetInBytes, data.lengthInBytes);

    await File(path).writeAsBytes(bytes, flush: true);
  } else {
    print("Opening existing database");
  }

  return await openDatabase(path, readOnly: true);
}

Future<Item> getItemWithId(String id) async {
  print("getItemWithId " + id);
  final db = await database;
  var response = await db
      .query("Items", where: "Id = ? OR RootId = ?", whereArgs: [id, id]);
  if (response.isEmpty) {
    print("Id not found");
  }
  return response.isNotEmpty ? Item.fromMap(response.first) : null;
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.red,
      ),
      home: MyHomePage(id: 'index.md'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, @required this.id}) : super(key: key);

  final String id;

  @override
  _MyHomePageState createState() => _MyHomePageState(id: this.id);
}

class _MyHomePageState extends State<MyHomePage> {
  _MyHomePageState({@required this.id});

  final String id;

  void setMarkdown(String md) {
    setState(() {
      markdown = md.replaceAllMapped(RegExp(r'<!--.*?-->'), (match) {
        return '';
      });
    });
  }

  String markdown = "";
  MarkdownStyleSheet styleSheet;

  @override
  void initState() {
    super.initState();

    ThemeData theme = ThemeData(
      brightness: Brightness.light,
      primaryColor: Colors.lightBlue[800],
      accentColor: Colors.cyan[600],
      fontFamily: 'LinuxLibertine',
      textTheme: TextTheme(
        headline: TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
        title: TextStyle(fontSize: 18.0, fontStyle: FontStyle.italic),
        body1: TextStyle(fontSize: 14.0, fontFamily: 'Hind'),
      ),
    );

    //styleSheet = MarkdownStyleSheet.fromTheme(theme);
    styleSheet = MarkdownStyleSheet(
        tableColumnWidth: IntrinsicColumnWidth(),
        tableCellsPadding: EdgeInsets.all(0.2));

    getItemWithId(this.id)
        .then((item) => setMarkdown(item.Markdown))
        .catchError((error) => print(error));
  }

  final Widget svg = SvgPicture.asset("assets/crystal-ball.svg",
    height: 20.0,
    width: 20.0,
    allowDrawingOutsideViewBox: true,
  );
  @override
  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(
        title: Text(widget.id),
      ),
      body: Markdown(
          data: markdown,
          styleSheet: styleSheet,
          onTapLink: (link) => Navigator.push(context,
              MaterialPageRoute(builder: (context) => MyHomePage(id: link)))),
      bottomNavigationBar: BottomNavigationBar(
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            title: Text('Home'),
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.business),
            title: Text('Business'),
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.business),
            //icon: svg,
            title: Text('School'),
            //activeIcon: Icon(Icons.category, color: Color(0xFFEF5123)),
          ),
        ],
      ),
    );
  }
}
