import 'package:aidedejeu_flutter/databases/database.dart';
import 'package:aidedejeu_flutter/models/filters.dart' as Filters;
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:sembast/sembast.dart';
import 'package:sembast/sembast_io.dart';
import 'package:sembast_web/sembast_web.dart';

class SembastDB extends BaseDB {
  static SembastDB _instance;
  static SembastDB get instance {
    if(_instance == null) {
      _instance = SembastDB();
    }
    return _instance;
  }

  Database _database;

  Future<Database> get database async {
    if (_database != null) return _database;
    _database = await getDatabaseInstance();
    return _database;
  }

  Future<Database> getDatabaseInstance() async {
    // File path to a file in the current directory
    String dbPath = 'library_sembast.db';
    DatabaseFactory dbFactory = databaseFactoryIo;

// We use the database factory to open the database
    return await dbFactory.openDatabase(dbPath);
  }

  @override
  Future<Item> getItemWithId(String id) {
    // TODO: implement getItemWithId
    throw UnimplementedError();
  }

  @override
  Future<List<BackgroundItem>> loadBackgrounds() {
    // TODO: implement loadBackgrounds
    throw UnimplementedError();
  }

  @override
  Future<Item> loadChildrenItems(Item item, List<Filters.Filter> filters) {
    // TODO: implement loadChildrenItems
    throw UnimplementedError();
  }

  @override
  Future<List<RaceItem>> loadRaces() {
    // TODO: implement loadRaces
    throw UnimplementedError();
  }

  @override
  Future<List<SubBackgroundItem>> loadSubBackgrounds(Item item) {
    // TODO: implement loadSubBackgrounds
    throw UnimplementedError();
  }

  @override
  Future<List<SubRaceItem>> loadSubRaces(RaceItem race) {
    // TODO: implement loadSubRaces
    throw UnimplementedError();
  }

  @override
  Future<List<T>> loadTypedItems<T extends Item>({String itemType, Item item}) {
    // TODO: implement loadTypedItems
    throw UnimplementedError();
  }
}
