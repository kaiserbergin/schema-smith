SHOW PRIVILEGES YIELD * 
RETURN role, access, collect([action, resource, graph, segment]) as privileges
ORDER BY role, access;