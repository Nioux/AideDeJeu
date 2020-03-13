import 'dart:io';

import 'package:aidedejeu_flutter/models/items.dart';
import 'package:flutter/services.dart';
import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';

import 'models/filters.dart';

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

Future<Item> loadChildrenItems(Item item, List<Filter> filters) async {
  print("getChildrenItems " + (item?.itemType ?? ""));
  if (item.itemType.endsWith("Items")) {
    String itemType =
    item.itemType.substring(0, item.itemType.length - 1);
    String family = "";
    if (item is FilteredItems) {
      family = item.family ?? "";
    }
    String whereFilter = "";
    if(filters != null) {
      filters.forEach((filter) {
        if(filter.selectedValues.isNotEmpty) {
          whereFilter = " AND (${filter.name} LIKE '%" + filter.selectedValues.join("%' OR ${filter.name} LIKE '%") + "%')";
        }
        if(filter.rangeValues != null && (filter.rangeValues.start > 0 || filter.rangeValues.end < filter.values.length - 1) ) {
          whereFilter = " AND ([${filter.name}] BETWEEN '${filter.values[filter.rangeValues.start.round()]}' AND '${filter.values[filter.rangeValues.end.round()]}')";
        }
      });
    }
    print(whereFilter);
    final db = await database;
    var response = await db.query(
        "Items",
        where: "ItemType = ? AND Family = ?" + whereFilter,
        whereArgs: [itemType, family],
        orderBy: "NormalizedName"
    );
    if (response.isEmpty) {
      print("Children not found");
    }
    item.children = response.isNotEmpty
        ? itemsFromMapList(response)
        : null;
  }
  return item;
}

Future<List<RaceItem>> loadRaces() async {
  final db = await database;
  var response = await db.query(
      "Items",
      where: "ItemType = 'RaceItem'",
      orderBy: "NormalizedName"
  );
  if (response.isNotEmpty) {
    return itemsFromMapList(response);
  }
  return null;
}

Future<List<SubRaceItem>> loadSubRaces(RaceItem race) async {
  final db = await database;
  var response = await db.query(
      "Items",
      where: "ItemType = 'SubRaceItem' AND ParentLink = ?",
      whereArgs: [race.id],
      orderBy: "NormalizedName"
  );
  if (response.isNotEmpty) {
    return itemsFromMapList(response);
  }
  return null;
}

Future<List<T>> loadTypedItems<T extends Item>({String itemType, Item item}) async {
  final db = await database;
  var response = await db.query(
      "Items",
      where: "ItemType = ?" + (item != null ? " AND ParentLink = ?" : ""),
      whereArgs: item != null ? [itemType, item.id] : [itemType],
      orderBy: "NormalizedName"
  );
  if (response.isNotEmpty) {
    return itemsFromMapList(response);
  }
  return null;
}

Future<List<BackgroundItem>> loadBackgrounds() async {
  return loadTypedItems<BackgroundItem>(itemType: "BackgroundItem");
}

Future<List<SubBackgroundItem>> loadSubBackgrounds(Item item) async {
  return loadTypedItems<SubBackgroundItem>(itemType: "SubBackgroundItem", item: item);
}

