#include <iostream>
#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <math.h>

GLuint Program;
GLuint VAO, VBO_position, VBO_color, Attrib_vertex, Attrib_color, Unif_model;
glm::mat4 model, scale, rotate, translate;
const int VERTEX_NUM = 360 + 2;

const char* VertexShaderSource = R"(
    #version 330 core
    in vec4 coord;
    in vec4 color;

    out vec4 vert_color;

    uniform mat4 model;

    void main() {
        gl_Position = model * coord;

        vert_color = color;
    }
)";

const char* FragmentShaderSource = R"(
    #version 330 core
    in vec4 vert_color;

    void main() {
        gl_FragColor  = vert_color;
    }
)";

void Init();
void Draw();

int main() {
    sf::Window window(sf::VideoMode(600, 600), "Task 12.4", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);

    window.setActive(true);

    glewInit();

    Init();

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }
            else if (event.type == sf::Event::Resized) {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case (sf::Keyboard::Left): scale = glm::scale(scale, glm::vec3(0.9, 1.0, 1.0)); break;
                case (sf::Keyboard::Right): scale = glm::scale(scale, glm::vec3(1.1, 1.0, 1.0)); break;
                case (sf::Keyboard::Up): scale = glm::scale(scale, glm::vec3(1.0, 1.1, 1.0)); break;
                case (sf::Keyboard::Down): scale = glm::scale(scale, glm::vec3(1.0, 0.9, 1.0)); break;
                default: break;
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();

        window.display();
    }

    glDeleteProgram(Program);
    glDeleteBuffers(1, &VBO_position);
    glDeleteBuffers(1, &VBO_color);

    return 0;
}

void checkOpenGLerror() {
    GLenum errCode;
    // https://www.khronos.org/opengl/wiki/OpenGL_Error
    if ((errCode = glGetError()) != GL_NO_ERROR)
        std::cout << "OpenGl error!: " << errCode << std::endl;
}

void ShaderLog(unsigned int shader) {
    int infologLen = 0;
    int charsWritten = 0;
    char* infoLog;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        infoLog = new char[infologLen];
        if (infoLog == NULL)
        {
            std::cout << "ERROR: Could not allocate InfoLog buffer" << std::endl;
            exit(1);
        }
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
        std::cout << "InfoLog: " << infoLog << "\n\n\n";
        delete[] infoLog;
    }
}


void InitVAO() {
    
    int i_rgb = (VERTEX_NUM - 2) / 3;
    int i_rgb_half = i_rgb / 2;

    glm::vec4 c = { 0.0, 0.0, 0.0, 1.0 };
    glm::vec4 cur_v = { 0.5, 0.0, 0.0, 1.0 };

    glm::vec4 vertexes[VERTEX_NUM];
    vertexes[0] = c;
    glm::mat4 vert_rotate = glm::rotate(glm::mat4(1.0f), glm::radians(1.0f), glm::vec3(0.0, 0.0, 1.0));

    for (int i = 1; i < VERTEX_NUM; i++)
    {
        vertexes[i] = cur_v;
        cur_v = vert_rotate * cur_v;
    }

    glm::vec4 colors[VERTEX_NUM];
    colors[0] = glm::vec4(1.0, 1.0, 1.0, 1.0);
    colors[VERTEX_NUM - 1] = glm::vec4(1.0, 0.0, 0.0, 1.0);
    glm::vec4 cur_c = glm::vec4(1.0, 0.0, 0.0, 1.0);

    float add = 1.0 / i_rgb_half;

    for (int i = 1; i < VERTEX_NUM - 1; i++)
    {
        colors[i] = cur_c;

        if (i < i_rgb_half)
            cur_c += glm::vec4(0.0, add, 0.0, 0.0); // red -> yellow
        else if (i < i_rgb)
            cur_c -= glm::vec4(add, 0.0, 0.0, 0.0); // yellow -> green
        else if (i < i_rgb + i_rgb_half)
            cur_c += glm::vec4(0.0, 0.0, add, 0.0); // green -> cyan
        else if (i < i_rgb * 2)
            cur_c -= glm::vec4(0.0, add, 0.0, 0.0); // cyan -> blue
        else if (i < i_rgb * 2 + i_rgb_half)
            cur_c += glm::vec4(add, 0.0, 0.0, 0.0); // blue -> purple
        else
            cur_c -= glm::vec4(0.0, 0.0, add, 0.0); // purple -> red
    }

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO_position);
    glGenBuffers(1, &VBO_color);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertexes), vertexes, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
    glVertexAttribPointer(Attrib_vertex, 4, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_vertex);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
    glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_color);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_color);

    checkOpenGLerror();
}


void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    std::cout << "vertex shader \n";
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &FragmentShaderSource, NULL);
    glCompileShader(fShader);
    std::cout << "fragment shader \n";
    ShaderLog(fShader);

    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    glLinkProgram(Program);

    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
        return;
    }

    Attrib_vertex = glGetAttribLocation(Program, "coord");
    if (Attrib_vertex == -1)
    {
        std::cout << "could not bind attrib coord" << std::endl;
        return;
    }

    Attrib_color = glGetAttribLocation(Program, "color");
    if (Attrib_color == -1)
    {
        std::cout << "could not bind attrib color" << std::endl;
        return;
    }

    Unif_model = glGetUniformLocation(Program, "model");
    if (Unif_model == -1)
    {
        std::cout << "could not bind unif model" << std::endl;
        return;
    }

    glDeleteShader(vShader);
    glDeleteShader(fShader);

    checkOpenGLerror();
}

void InitMatrix()
{
    scale = glm::mat4(1.0f);
    rotate = glm::mat4(1.0f);
    translate = glm::mat4(1.0f);
}

void Init() {
    InitShader();
    InitVAO();
    InitMatrix();
}

void Draw() {
    glUseProgram(Program);
    glBindVertexArray(VAO);

    model = scale * translate * rotate;

    glUniformMatrix4fv(Unif_model, 1, GL_FALSE, glm::value_ptr(model));
    glDrawArrays(GL_TRIANGLE_FAN, 0, VERTEX_NUM);

    glBindVertexArray(0);
    glUseProgram(0);

    checkOpenGLerror();
}