import json
from types import SimpleNamespace


def convert_schema_to_obj(neo_schema):
    neo_json = json.dumps(neo_schema, sort_keys=False)
    return json.loads(neo_json, object_hook=lambda d: SimpleNamespace(**d))
