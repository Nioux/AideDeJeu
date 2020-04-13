import 'dart:convert';

import 'package:http/http.dart' as http;

class MyWebApi {
  final _baseUrl = "dummy.test.org";

  Future<MyResponse> fetchData(
      String id) async {
    String apiPath = "/my_path/" + id;
    final response = await http.get(Uri.https(_baseUrl, apiPath));
    if (response.statusCode == 200) {
      print(response.body);
      return MyResponse.fromJson(json.decode(response.body));
    } else {
      throw Exception('Failed to load data');
    }
  }
}
