import 'dart:io';

import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/services.dart';
import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';

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
  exists = false;
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
  var response = await db.query(
      "Items",
      where: "Id = ? OR RootId = ?",
      whereArgs: [id, id]
  );
  if (response.isEmpty) {
    print("Id not found");
  }
  return response.isNotEmpty ? itemFromMap(response.first) : null;
}

Future<Item> loadChildrenItems(Item item) async {
  print("getChildrenItems " + (item?.ItemType ?? ""));
  if (item.ItemType.endsWith("Items")) {
    String itemType =
    item.ItemType.substring(0, item.ItemType.length - 1);
    String family = "";
    if (item is FilteredItems) {
      family = (item as FilteredItems)?.Family ?? "";
    }
    final db = await database;
    var response = await db.query(
        "Items",
        where: "ItemType = ? AND Family = ?",
        whereArgs: [itemType, family],
        orderBy: "NormalizedName"
    );
    if (response.isEmpty) {
      print("Children not found");
    }
    item.Children = response.isNotEmpty
        ? itemsFromMapList(response)
        : null;
  }
  return item;
}
