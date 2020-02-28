import 'package:flutter/material.dart';

class Item {
  String id;
  String rootId;
  String parentLink;
  String name;
  String normalizedName;
  String parentName;
  int nameLevel;
  String alias;
  String aliasText;
  String normalizedAlias;
  String source;
  String markdown;
  String fullText;
  String itemType;
  List<Item> children;

  Item({this.id, this.name, this.alias, this.markdown, this.itemType});

  Item.fromMap(Map<String, dynamic> map) {
    this.id = map["Id"];
    this.rootId = map["RootId"];
    this.name = map["Name"];
    this.alias = map["AltName"];
    this.aliasText = map["AltNameText"];
    this.markdown = map["Markdown"];
    this.itemType = map["ItemType"];
  }

  /*factory Item.fromMap(Map<String, dynamic> json) => new Item(
    Id: json["Id"],
    Name: json["Name"],
    Alias: json["AltName"],
    Markdown: json["Markdown"],
    Discriminator: json["Discriminator"],
  );*/

  Map<String, dynamic> toMap() => {
    "Id": id,
    "RootId": rootId,
    "Name": name,
    "AltName": alias,
    "AltNameText": aliasText,
    "Markdown": markdown,
    "ItemType": itemType,
  };
}

class MonsterItem extends Item {
  String family;
  String type;
  String size;
  String alignment;
  String terrain;
  String legendary;
  String armorClass;
  String hitPoints;
  String speed;
  String strength;
  String dexterity;
  String constitution;
  String intelligence;
  String wisdom;
  String charisma;
  String savingThrows;
  String skills;
  String damageVulnerabilities;
  String damageImmunities;
  String conditionImmunities;
  String damageResistances;
  String senses;
  String languages;
  String challenge;
  int xp;

  MonsterItem.fromMap(Map<String, dynamic> map)
    : super.fromMap(map) {
    this.family = map["Family"];
    this.type = map["Type"];
    this.size = map["Size"];
    this.alignment = map["Alignment"];
    this.terrain = map["Terrain"];
    this.legendary = map["Legendary"];
    this.armorClass = map["ArmorClass"];
    this.hitPoints = map["HitPoints"];
    this.speed = map["Speed"];
    this.strength = map["Strength"];
    this.dexterity = map["Dexterity"];
    this.constitution = map["Constitution"];
    this.intelligence = map["Intelligence"];
    this.wisdom = map["Wisdom"];
    this.charisma = map["Charisma"];
    this.savingThrows = map["SavingThrows"];
    this.skills = map["Skills"];
    this.damageVulnerabilities = map["DamageVulnerabilities"];
    this.damageImmunities = map["DamageImmunities"];
    this.conditionImmunities = map["ConditionImmunities"];
    this.damageResistances = map["DamageResistances"];
    this.senses = map["Senses"];
    this.languages = map["Languages"];
    this.challenge = map["Challenge"];
    this.xp = map["XP"];
  }
}

class Items extends Item {

  Items.fromMap(Map<String, dynamic> map)
      : super.fromMap(map) {
  }
}

abstract class FilteredItems extends Items {
  String family;

  FilteredItems.fromMap(Map<String, dynamic> map)
      : super.fromMap(map) {
    this.family = map["Family"];
  }

  List<Filter> toFilterList();
}

enum FilterType {
  Choices, Range
}
class Filter {
  String name;
  FilterType type;
  List<String> values;
  Set<String> selectedValues = Set<String>();
  RangeValues rangeValues;

  Filter({this.name, this.type, this.values}) {
    rangeValues = RangeValues(0, values.length.toDouble() - 1);
  }
}

class MonsterItems extends FilteredItems {
  Filter types;
  Filter challenges;
  Filter sizes;
  Filter sources;
  Filter terrains;

  MonsterItems.fromMap(Map<String, dynamic> map)
      : super.fromMap(map) {
    this.types = Filter(name: "Type", type: FilterType.Choices, values: map["Types"].toString().split("|"));
    this.challenges = Filter(name: "Challenge", type: FilterType.Range, values: map["Challenges"].toString().split("|"));
    this.sizes = Filter(name: "Size", type: FilterType.Range, values: map["Sizes"].toString().split("|"));;
    this.sources = Filter(name: "Source", type: FilterType.Choices, values: map["Sources"].toString().split("|"));
    this.terrains = Filter(name: "Terrain", type: FilterType.Choices, values: map["Terrains"].toString().split("|"));
  }

//  Map<String, dynamic> toMap() => {
  //"Id": Id,
  List<Filter> toFilterList() => {
    types,
    challenges,
    sizes,
    sources,
    terrains,
  }.toList();

}

Item itemFromMap(Map<String, dynamic> map) {
  switch(map["ItemType"]) {
    case "MonsterItem": return MonsterItem.fromMap(map);
    case "MonsterItems": return MonsterItems.fromMap(map);
  }
  return Item.fromMap(map);
}

List<Item> itemsFromMapList(List<Map<String, dynamic>> mapList) {
  return List.generate(mapList.length, (i) {
    return itemFromMap(mapList[i]);
  });
}