// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'test.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Test _$TestFromJson(Map<String, dynamic> json) {
  return Test()
    ..id = json['id'] as String
    ..rootId = json['rootId'] as String
    ..parentLink = json['parentLink'] as String
    ..name = json['name'] as String;
}

Map<String, dynamic> _$TestToJson(Test instance) => <String, dynamic>{
      'id': instance.id,
      'rootId': instance.rootId,
      'parentLink': instance.parentLink,
      'name': instance.name,
    };
