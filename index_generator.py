__BTREE_INDEX = "BTREE"
__TEXT_INDEX = "TEXT"
__POINT_INDEX = "POINT"
__LOOKUP_INDEX = "LOOKUP"
__RANGE_INDEX = "RANGE"


def __properties_str(constraint, prefix_var_name):
    query = "\n("
    prop_count = 0
    for prop in constraint.entity.properties:
        query += "\n  " if prop_count == 0 else ",\n  "
        query += f"{prefix_var_name}.{prop}"
        prop_count += 1

    query += "\n)"

    return query


def __for_statment_str(index, var_name):
    query = ""

    if index.entity.type == "node":
        query += f"\nFOR ({var_name}:{index.entity.name}) ON "
    elif index.entity.type == "relationship":
        query += f"\nFOR ()-[{var_name}:{index.entity.name}]-() ON "

    return query


def __index_str(index, index_count):
    entity_var = f"{index.entity.name}{index_count}"

    query = f"\nCREATE {__index_dict[index.type]} INDEX IF NOT EXISTS "
    query += __for_statment_str(index, entity_var)
    query += __properties_str(index, entity_var)
    query += ";\n"

    return query


__index_dict = {
    "b-tree": __BTREE_INDEX,
    "text": __TEXT_INDEX,
    "point": __POINT_INDEX,
    "lookup": __LOOKUP_INDEX,
    "range": __RANGE_INDEX
}


def generate_indexes_query(schema):
    query = "\n"
    index_count = 0

    if hasattr(schema, 'indexes'):
        for index in schema.indexes:
            index_count += 1
            query += __index_str(index, index_count)

    return query
