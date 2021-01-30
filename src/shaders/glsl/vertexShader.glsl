#version 330 core

layout (location = 0) in vec2 position;
layout (location = 1) in vec3 color;

out vec3 frag_color;
out vec3 vertexPosition;
out float seed;
uniform float s;
uniform float pointSize;

void main() {
    frag_color = color;
    vertexPosition = vec3(position, 0.0);
    seed = s;
    gl_Position = vec4(vec3(position, 0.0), 1.0);
    gl_PointSize = pointSize;
}