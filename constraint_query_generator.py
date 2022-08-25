

def __require_normal_properties_str(constraint, prefix_var_name):
    query = "\nREQUIRE ("
    prop_count = 0
    for prop in constraint.entity.properties:
        query += "\n  " if prop_count == 0 else ",\n  "
        query += f"{prefix_var_name}.{prop}"
        prop_count += 1

    query += "\n) "

    return query


def __require_existence_properties_str(constraint, prefix_var_name):
    query = ""

    for prop in constraint.entity.properties:
        query += f"\nREQUIRE {prefix_var_name}.{prop} IS NOT NULL"

    return query


def __require_section_str(constraint, prefix_var_name):
    query = __require_normal_properties_str(constraint, prefix_var_name)
    query += __constraint_suffix_dict[constraint.type]
    return query


def __node_constraint_for_str(constraint, node_var_name):
    return f"\nFOR ({node_var_name}:{constraint.entity.name})"


def __node_key_constraint_str(constraint, constraint_count):
    if constraint.type == "existence":
        query = __node_existence_constraint_str(constraint, constraint_count)
    else:
        node_var_name = f"nc{constraint_count}"
        query = f"\nCREATE CONSTRAINT {constraint.name} IF NOT EXISTS"
        query += __node_constraint_for_str(constraint, node_var_name)
        query += __require_normal_properties_str(constraint, node_var_name)
        query += __constraint_suffix_dict[constraint.type]
        query += ";\n"

    return query


def __relationship_constraint_str(constraint, constraint_count):
    query = ""

    for prop in constraint.entity.properties:
        rel_var_name = f"rc{constraint_count}"
        query += f"\nCREATE CONSTRAINT {constraint.name} IF NOT EXISTS"
        query += f"\nFOR ()-[{rel_var_name}:{constraint.entity.name}]-()"
        query += f"\nREQUIRE {rel_var_name}.{prop} IS NOT NULL"
        query += ";\n"

    return query


def __node_existence_constraint_str(constraint, constraint_count):
    query = ""

    for prop in constraint.entity.properties:
        node_var_name = f"nc{constraint_count}"
        query += f"\nCREATE CONSTRAINT {constraint.name} IF NOT EXISTS"
        query += f"\nFOR ({node_var_name}:{constraint.entity.name})"
        query += f"\nREQUIRE {node_var_name}.{prop} IS NOT NULL"
        query += ";\n"

    return query


__constraint_suffix_dict = {
    "node-key": "IS NODE KEY",
    "unique": "IS UNIQUE",
    "existence": "IS NOT NULL"
}


def generate_constraints_query(schema):
    query = "\n"
    constraint_count = 0

    if hasattr(schema, 'constraints'):
        for constraint in schema.constraints:
            constraint_count += 1
            if constraint.entity.type == "node":
                query += __node_key_constraint_str(constraint, constraint_count)
            elif constraint.entity.type == "relationship":
                query += __relationship_constraint_str(constraint, constraint_count)

    return query
