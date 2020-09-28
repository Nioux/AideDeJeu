import 'package:json_annotation/json_annotation.dart';

part 'test.g.dart';

@JsonSerializable(explicitToJson: true)
class Test {
  String id;
  String rootId;
  String parentLink;
  String name;

  Test();

  factory Test.fromJson(Map<String, dynamic> map) => _$TestFromJson(map);
  Map<String, dynamic> toJson() => _$TestToJson(this);
}
