// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'items.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Item _$ItemFromJson(Map<String, dynamic> json) {
  return Item()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$ItemToJson(Item instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

GenericItem _$GenericItemFromJson(Map<String, dynamic> json) {
  return GenericItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$GenericItemToJson(GenericItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

MonsterItem _$MonsterItemFromJson(Map<String, dynamic> json) {
  return MonsterItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String
    ..type = json['type'] as String
    ..size = json['size'] as String
    ..alignment = json['alignment'] as String
    ..terrain = json['terrain'] as String
    ..legendary = json['legendary'] as String
    ..armorClass = json['armorClass'] as String
    ..hitPoints = json['hitPoints'] as String
    ..speed = json['speed'] as String
    ..strength = json['strength'] as String
    ..dexterity = json['dexterity'] as String
    ..constitution = json['constitution'] as String
    ..intelligence = json['intelligence'] as String
    ..wisdom = json['wisdom'] as String
    ..charisma = json['charisma'] as String
    ..savingThrows = json['savingThrows'] as String
    ..skills = json['skills'] as String
    ..damageVulnerabilities = json['damageVulnerabilities'] as String
    ..damageImmunities = json['damageImmunities'] as String
    ..conditionImmunities = json['conditionImmunities'] as String
    ..damageResistances = json['damageResistances'] as String
    ..senses = json['senses'] as String
    ..languages = json['languages'] as String
    ..challenge = json['challenge'] as String
    ..xp = json['xp'] as int;
}

Map<String, dynamic> _$MonsterItemToJson(MonsterItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
      'type': instance.type,
      'size': instance.size,
      'alignment': instance.alignment,
      'terrain': instance.terrain,
      'legendary': instance.legendary,
      'armorClass': instance.armorClass,
      'hitPoints': instance.hitPoints,
      'speed': instance.speed,
      'strength': instance.strength,
      'dexterity': instance.dexterity,
      'constitution': instance.constitution,
      'intelligence': instance.intelligence,
      'wisdom': instance.wisdom,
      'charisma': instance.charisma,
      'savingThrows': instance.savingThrows,
      'skills': instance.skills,
      'damageVulnerabilities': instance.damageVulnerabilities,
      'damageImmunities': instance.damageImmunities,
      'conditionImmunities': instance.conditionImmunities,
      'damageResistances': instance.damageResistances,
      'senses': instance.senses,
      'languages': instance.languages,
      'challenge': instance.challenge,
      'xp': instance.xp,
    };

SpellItem _$SpellItemFromJson(Map<String, dynamic> json) {
  return SpellItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String
    ..level = json['level'] as String
    ..type = json['type'] as String
    ..ritual = json['ritual'] as String
    ..castingTime = json['castingTime'] as String
    ..range = json['range'] as String
    ..components = json['components'] as String
    ..verbalComponent = json['verbalComponent'] as String
    ..somaticComponent = json['somaticComponent'] as String
    ..materialComponent = json['materialComponent'] as String
    ..concentration = json['concentration'] as String
    ..duration = json['duration'] as String
    ..classes = json['classes'] as String;
}

Map<String, dynamic> _$SpellItemToJson(SpellItem instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
      'level': instance.level,
      'type': instance.type,
      'ritual': instance.ritual,
      'castingTime': instance.castingTime,
      'range': instance.range,
      'components': instance.components,
      'verbalComponent': instance.verbalComponent,
      'somaticComponent': instance.somaticComponent,
      'materialComponent': instance.materialComponent,
      'concentration': instance.concentration,
      'duration': instance.duration,
      'classes': instance.classes,
    };

Items _$ItemsFromJson(Map<String, dynamic> json) {
  return Items()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$ItemsToJson(Items instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

FilteredItems _$FilteredItemsFromJson(Map<String, dynamic> json) {
  return FilteredItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String;
}

Map<String, dynamic> _$FilteredItemsToJson(FilteredItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
    };

MonsterItems _$MonsterItemsFromJson(Map<String, dynamic> json) {
  return MonsterItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String
    ..typesString = json['typesString'] as String
    ..challengesString = json['challengesString'] as String
    ..sizesString = json['sizesString'] as String
    ..sourcesString = json['sourcesString'] as String
    ..terrainsString = json['terrainsString'] as String;
}

Map<String, dynamic> _$MonsterItemsToJson(MonsterItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
      'typesString': instance.typesString,
      'challengesString': instance.challengesString,
      'sizesString': instance.sizesString,
      'sourcesString': instance.sourcesString,
      'terrainsString': instance.terrainsString,
    };

SpellItems _$SpellItemsFromJson(Map<String, dynamic> json) {
  return SpellItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String;
}

Map<String, dynamic> _$SpellItemsToJson(SpellItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
    };

RaceItem _$RaceItemFromJson(Map<String, dynamic> json) {
  return RaceItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..fullName = json['fullName'] as String
    ..hasSubRaces = json['hasSubRaces'] as bool
    ..strengthBonus = json['strengthBonus'] as String
    ..dexterityBonus = json['dexterityBonus'] as String
    ..constitutionBonus = json['constitutionBonus'] as String
    ..intelligenceBonus = json['intelligenceBonus'] as String
    ..wisdomBonus = json['wisdomBonus'] as String
    ..charismaBonus = json['charismaBonus'] as String
    ..dispatchedBonus = json['dispatchedBonus'] as String
    ..maxDispatchedStrengthBonus = json['maxDispatchedStrengthBonus'] as String
    ..maxDispatchedDexterityBonus =
        json['maxDispatchedDexterityBonus'] as String
    ..maxDispatchedConstitutionBonus =
        json['maxDispatchedConstitutionBonus'] as String
    ..maxDispatchedIntelligenceBonus =
        json['maxDispatchedIntelligenceBonus'] as String
    ..maxDispatchedWisdomBonus = json['maxDispatchedWisdomBonus'] as String
    ..maxDispatchedCharismaBonus = json['maxDispatchedCharismaBonus'] as String
    ..abilityScoreIncrease = json['abilityScoreIncrease'] as String
    ..age = json['age'] as String
    ..alignment = json['alignment'] as String
    ..size = json['size'] as String
    ..speed = json['speed'] as String
    ..darkvision = json['darkvision'] as String
    ..languages = json['languages'] as String;
}

Map<String, dynamic> _$RaceItemToJson(RaceItem instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'fullName': instance.fullName,
      'hasSubRaces': instance.hasSubRaces,
      'strengthBonus': instance.strengthBonus,
      'dexterityBonus': instance.dexterityBonus,
      'constitutionBonus': instance.constitutionBonus,
      'intelligenceBonus': instance.intelligenceBonus,
      'wisdomBonus': instance.wisdomBonus,
      'charismaBonus': instance.charismaBonus,
      'dispatchedBonus': instance.dispatchedBonus,
      'maxDispatchedStrengthBonus': instance.maxDispatchedStrengthBonus,
      'maxDispatchedDexterityBonus': instance.maxDispatchedDexterityBonus,
      'maxDispatchedConstitutionBonus': instance.maxDispatchedConstitutionBonus,
      'maxDispatchedIntelligenceBonus': instance.maxDispatchedIntelligenceBonus,
      'maxDispatchedWisdomBonus': instance.maxDispatchedWisdomBonus,
      'maxDispatchedCharismaBonus': instance.maxDispatchedCharismaBonus,
      'abilityScoreIncrease': instance.abilityScoreIncrease,
      'age': instance.age,
      'alignment': instance.alignment,
      'size': instance.size,
      'speed': instance.speed,
      'darkvision': instance.darkvision,
      'languages': instance.languages,
    };

SubRaceItem _$SubRaceItemFromJson(Map<String, dynamic> json) {
  return SubRaceItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..fullName = json['fullName'] as String
    ..hasSubRaces = json['hasSubRaces'] as bool
    ..strengthBonus = json['strengthBonus'] as String
    ..dexterityBonus = json['dexterityBonus'] as String
    ..constitutionBonus = json['constitutionBonus'] as String
    ..intelligenceBonus = json['intelligenceBonus'] as String
    ..wisdomBonus = json['wisdomBonus'] as String
    ..charismaBonus = json['charismaBonus'] as String
    ..dispatchedBonus = json['dispatchedBonus'] as String
    ..maxDispatchedStrengthBonus = json['maxDispatchedStrengthBonus'] as String
    ..maxDispatchedDexterityBonus =
        json['maxDispatchedDexterityBonus'] as String
    ..maxDispatchedConstitutionBonus =
        json['maxDispatchedConstitutionBonus'] as String
    ..maxDispatchedIntelligenceBonus =
        json['maxDispatchedIntelligenceBonus'] as String
    ..maxDispatchedWisdomBonus = json['maxDispatchedWisdomBonus'] as String
    ..maxDispatchedCharismaBonus = json['maxDispatchedCharismaBonus'] as String
    ..abilityScoreIncrease = json['abilityScoreIncrease'] as String
    ..age = json['age'] as String
    ..alignment = json['alignment'] as String
    ..size = json['size'] as String
    ..speed = json['speed'] as String
    ..darkvision = json['darkvision'] as String
    ..languages = json['languages'] as String
    ..parentRaceId = json['parentRaceId'] as String;
}

Map<String, dynamic> _$SubRaceItemToJson(SubRaceItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'fullName': instance.fullName,
      'hasSubRaces': instance.hasSubRaces,
      'strengthBonus': instance.strengthBonus,
      'dexterityBonus': instance.dexterityBonus,
      'constitutionBonus': instance.constitutionBonus,
      'intelligenceBonus': instance.intelligenceBonus,
      'wisdomBonus': instance.wisdomBonus,
      'charismaBonus': instance.charismaBonus,
      'dispatchedBonus': instance.dispatchedBonus,
      'maxDispatchedStrengthBonus': instance.maxDispatchedStrengthBonus,
      'maxDispatchedDexterityBonus': instance.maxDispatchedDexterityBonus,
      'maxDispatchedConstitutionBonus': instance.maxDispatchedConstitutionBonus,
      'maxDispatchedIntelligenceBonus': instance.maxDispatchedIntelligenceBonus,
      'maxDispatchedWisdomBonus': instance.maxDispatchedWisdomBonus,
      'maxDispatchedCharismaBonus': instance.maxDispatchedCharismaBonus,
      'abilityScoreIncrease': instance.abilityScoreIncrease,
      'age': instance.age,
      'alignment': instance.alignment,
      'size': instance.size,
      'speed': instance.speed,
      'darkvision': instance.darkvision,
      'languages': instance.languages,
      'parentRaceId': instance.parentRaceId,
    };

RaceItems _$RaceItemsFromJson(Map<String, dynamic> json) {
  return RaceItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String;
}

Map<String, dynamic> _$RaceItemsToJson(RaceItems instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
    };

OriginItem _$OriginItemFromJson(Map<String, dynamic> json) {
  return OriginItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..regionsOfOrigin = json['regionsOfOrigin'] as String
    ..mainLanguages = json['mainLanguages'] as String
    ..aspirations = json['aspirations'] as String
    ..availableSkills = json['availableSkills'] as String;
}

Map<String, dynamic> _$OriginItemToJson(OriginItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'regionsOfOrigin': instance.regionsOfOrigin,
      'mainLanguages': instance.mainLanguages,
      'aspirations': instance.aspirations,
      'availableSkills': instance.availableSkills,
    };

OriginItems _$OriginItemsFromJson(Map<String, dynamic> json) {
  return OriginItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['family'] as String;
}

Map<String, dynamic> _$OriginItemsToJson(OriginItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'family': instance.family,
    };

BackgroundItem _$BackgroundItemFromJson(Map<String, dynamic> json) {
  return BackgroundItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..skillProficiencies = json['skillProficiencies'] as String
    ..masteredTools = json['masteredTools'] as String
    ..masteredLanguages = json['masteredLanguages'] as String
    ..equipment = json['equipment'] as String;
}

Map<String, dynamic> _$BackgroundItemToJson(BackgroundItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'skillProficiencies': instance.skillProficiencies,
      'masteredTools': instance.masteredTools,
      'masteredLanguages': instance.masteredLanguages,
      'equipment': instance.equipment,
    };

SubBackgroundItem _$SubBackgroundItemFromJson(Map<String, dynamic> json) {
  return SubBackgroundItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..alias = json['Alias'] as String
    ..aliasText = json['AliasText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..skillProficiencies = json['skillProficiencies'] as String
    ..masteredTools = json['masteredTools'] as String
    ..masteredLanguages = json['masteredLanguages'] as String
    ..equipment = json['equipment'] as String;
}

Map<String, dynamic> _$SubBackgroundItemToJson(SubBackgroundItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'Alias': instance.alias,
      'AliasText': instance.aliasText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'skillProficiencies': instance.skillProficiencies,
      'masteredTools': instance.masteredTools,
      'masteredLanguages': instance.masteredLanguages,
      'equipment': instance.equipment,
    };
