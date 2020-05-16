import 'package:aidedejeu_flutter/models/filters.dart';
import 'package:aidedejeu_flutter/models/items.dart';

abstract class BaseDB {
  Future<List<Item>> loadAllItems();

  Future<Item> getItemWithId(String id);

  Future<Item> loadChildrenItems(Item item, List<Filter> filters);

  Future<List<RaceItem>> loadRaces();

  Future<List<SubRaceItem>> loadSubRaces(RaceItem race);

  Future<List<OriginItem>> loadOrigins();

  Future<List<T>> loadTypedItems<T extends Item>({String itemType, Item item});

  Future<List<BackgroundItem>> loadBackgrounds();

  Future<List<SubBackgroundItem>> loadSubBackgrounds(Item item);
}
