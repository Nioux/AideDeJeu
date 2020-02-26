class Item {
  String Id;
  String RootId;
  String ParentLink;
  String Name;
  String NormalizedName;
  String ParentName;
  int NameLevel;
  String Alias;
  String AliasText;
  String NormalizedAlias;
  String Source;
  String Markdown;
  String FullText;
  String ItemType;
  List<Item> Children;

  Item({this.Id, this.Name, this.Alias, this.Markdown, this.ItemType});

  Item.fromMap(Map<String, dynamic> map) {
    this.Id = map["Id"];
    this.Name = map["Name"];
    this.Alias = map["AltName"];
    this.AliasText = map["AltNameText"];
    this.Markdown = map["Markdown"];
    this.ItemType = map["ItemType"];
  }

  /*factory Item.fromMap(Map<String, dynamic> json) => new Item(
    Id: json["Id"],
    Name: json["Name"],
    Alias: json["AltName"],
    Markdown: json["Markdown"],
    Discriminator: json["Discriminator"],
  );*/

  Map<String, dynamic> toMap() => {
    "Id": Id,
    "RootId": Id,
    "Name": Name,
    "AltName": Alias,
    "AltNameText": AliasText,
    "Markdown": Markdown,
    "ItemType": ItemType,
  };
}

class MonsterItem extends Item {
  String Family;
  String Type;
  String Size;
  String Alignment;
  String Terrain;
  String Legendary;
  String ArmorClass;
  String HitPoints;
  String Speed;
  String Strength;
  String Dexterity;
  String Constitution;
  String Intelligence;
  String Wisdom;
  String Charisma;
  String SavingThrows;
  String Skills;
  String DamageVulnerabilities;
  String DamageImmunities;
  String ConditionImmunities;
  String DamageResistances;
  String Senses;
  String Languages;
  String Challenge;
  int XP;

  MonsterItem.fromMap(Map<String, dynamic> map)
    : super.fromMap(map) {
    this.Family = map["Family"];
    this.Type = map["Type"];
    this.Size = map["Size"];
    this.Alignment = map["Alignment"];
    this.Terrain = map["Terrain"];
    this.Legendary = map["Legendary"];
    this.ArmorClass = map["ArmorClass"];
    this.HitPoints = map["HitPoints"];
    this.Speed = map["Speed"];
    this.Strength = map["Strength"];
    this.Dexterity = map["Dexterity"];
    this.Constitution = map["Constitution"];
    this.Intelligence = map["Intelligence"];
    this.Wisdom = map["Wisdom"];
    this.Charisma = map["Charisma"];
    this.SavingThrows = map["SavingThrows"];
    this.Skills = map["Skills"];
    this.DamageVulnerabilities = map["DamageVulnerabilities"];
    this.DamageImmunities = map["DamageImmunities"];
    this.ConditionImmunities = map["ConditionImmunities"];
    this.DamageResistances = map["DamageResistances"];
    this.Senses = map["Senses"];
    this.Languages = map["Languages"];
    this.Challenge = map["Challenge"];
    this.XP = map["XP"];
  }
}

class Items extends Item {

  Items.fromMap(Map<String, dynamic> map)
      : super.fromMap(map) {
  }
}

abstract class FilteredItems extends Items {
  String Family;

  FilteredItems.fromMap(Map<String, dynamic> map)
      : super.fromMap(map) {
    this.Family = map["Family"];
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
  Filter({this.name, this.type, this.values});
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
    this.challenges = Filter(name: "Dangerosité", type: FilterType.Range, values: map["Challenges"].toString().split("|"));
    this.sizes = Filter(name: "Taille", type: FilterType.Range, values: map["Sizes"].toString().split("|"));;
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